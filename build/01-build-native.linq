<Query Kind="Program">
  <NuGetReference>SharpZipLib</NuGetReference>
  <NuGetReference>System.Linq.Async</NuGetReference>
  <Namespace>ICSharpCode.SharpZipLib.Tar</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load ".\00-common"

async Task Main()
{
	await SetupAsync(QueryCancelToken);
	// from website
	//await new WindowsNugetSource("win-x64", "win64", new LibNames("libraw.dll"), @"https://io.starworks.cc:88/cv-public/2023/LibRaw-0.21.1-Win64.zip", "Sdcb.LibRaw", deps: new string[0])
	//	.Process(QueryCancelToken);
	// from vcpkg local
	LibNames libs = new LibNames("raw_r.dll", "lcms2.dll", "zlib1.dll", "jpeg8.dll");
	await new WindowsNugetSource("win-x64", "win64", libs, @"C:\_\3rd\vcpkg\installed\x64-windows\bin", "Sdcb.LibRaw", deps: new string[0])
		.Process(QueryCancelToken);
	await new WindowsNugetSource("win-x86", "win32", libs, @"C:\_\3rd\vcpkg\installed\x86-windows\bin", "Sdcb.LibRaw", deps: new string[0])
		.Process(QueryCancelToken);
}

static string BuildNuspec(string[] libs, string rid, string titleRid, string folder, string pkgName, string[] deps)
{
	// props
	{
		XDocument props = XDocument.Parse($"""
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
	<PropertyGroup>
	</PropertyGroup>
	<ItemGroup Condition="$(TargetFrameworkVersion.StartsWith('v4')) Or $(TargetFramework.StartsWith('net4'))">
	</ItemGroup>
</Project>
""");
		string normalizedName = pkgName.Replace(".", "") + "Dlls";
		string ns = props.Root.GetDefaultNamespace().NamespaceName;
		XmlNamespaceManager nsr = new(new NameTable());
		nsr.AddNamespace("p", ns);

		string platform = rid.Split("-").Last();

		XElement? itemGroup = props.XPathSelectElement("/p:Project/p:ItemGroup", nsr);
		if (itemGroup == null) throw new Exception($"{nameof(itemGroup)} invalid in props file.");
		itemGroup.Add(
		libs
			.Select(x => Path.GetFileName(x))
			.Select(x =>
				new XElement(XName.Get("Content", ns), new XAttribute("Include", $@"$({normalizedName})\{rid}\native\{x}"),
					new XElement(XName.Get("Link", ns), @$"dll\{platform}\{x}"),
					new XElement(XName.Get("CopyToOutputDirectory", ns), "PreserveNewest")))
				);

		XElement? propGroup = props.XPathSelectElement("/p:Project/p:PropertyGroup", nsr);
		if (propGroup == null) throw new Exception($"{nameof(propGroup)} invalid in props file.");
		propGroup.Add(new XElement(XName.Get(normalizedName, ns), @"$(MSBuildThisFileDirectory)..\..\runtimes"));
		props.Save(Path.Combine(folder, $"{titleRid}.props"));
	}

	// nuspec
	{
		string masterName = "LibRaw";
		XDocument nuspec = XDocument.Parse($"""
<?xml version="1.0" encoding="utf-8"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
    <metadata>
        <id>{pkgName}.runtime.{titleRid}</id>
        <version>2.2.1</version>
        <title>{masterName} native bindings for {titleRid}</title>
        <authors>sdcb</authors>
        <license type="expression">(LGPL-2.1-only OR CDDL-1.0)</license>
        <projectUrl>https://github.com/sdcb/{pkgName}</projectUrl>
        <icon>images\icon.png</icon>
        <requireLicenseAcceptance>true</requireLicenseAcceptance>
        <description>Native binding for {masterName} to work on {titleRid}.</description>
        <summary>Native binding for {masterName} to work on {titleRid}.</summary>
        <releaseNotes></releaseNotes>
        <copyright>Copyright {DateTime.Now.Year}</copyright>
        <tags>libraw;raw;image;sony;canon;nikon;arw;cr3;nef;linqpad-samples</tags>
        <repository type="git" url="https://github.com/sdcb/{pkgName}.git" />
        <dependencies>
        </dependencies>
        <frameworkAssemblies>
        </frameworkAssemblies>
    </metadata>
    <files>
        <file src="..\{pkgName}.png" target="images\icon.png" />
    </files>
</package>
""");

		string ns = nuspec.Root.GetDefaultNamespace().NamespaceName;
		XmlNamespaceManager nsr = new(new NameTable());
		nsr.AddNamespace("p", ns);

		XElement? files = nuspec.XPathSelectElement("/p:package/p:files", nsr);
		if (files == null) throw new Exception($"{nameof(files)} invalid in nuspec file.");
		files.Add(libs.Select(x => new XElement(
			XName.Get("file", ns),
			new XAttribute("src", x.Split('\\', 2)[1]),
			new XAttribute("target", @$"runtimes\{rid}\native"))));
		files.Add(new[] { "net", "netstandard", "netcoreapp" }.Select(x => new XElement(
			XName.Get("file", ns),
			new XAttribute("src", $"{titleRid}.props"),
			new XAttribute("target", @$"build\{x}\{pkgName}.runtime.{titleRid}.props"))));

		if (deps.Any())
		{
			XElement? dependencies = nuspec.XPathSelectElement("/p:package/p:metadata/p:dependencies", nsr);
			if (dependencies == null) throw new Exception($"{nameof(dependencies)} invalid in nuspec file.");
			dependencies.Add(deps.Select(depId => new XElement(XName.Get("dependency", ns),
				new XAttribute("id", $"{depId}.runtime.{titleRid}"),
				new XAttribute("version", Projects.First(x => x.name == depId).version))));
		}

		string destinationFile = Path.Combine(folder, $"{titleRid}.nuspec");
		nuspec.Save(destinationFile);
		return destinationFile;
	}
}

public record WindowsNugetSource(string rid, string titleRid, LibNames libNames, string url, string pkgName, string[] deps) : NupkgBuildSource(rid, titleRid, libNames, url, pkgName, deps)
{
	protected override async Task Decompress(CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(CLibFolder);
		if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
		{
			using HttpClient http = new HttpClient();
			Stream compressed = await http.GetStreamAsync(url);
			using ZipArchive zip = new ZipArchive(compressed);

			foreach (ZipArchiveEntry entry in zip.Entries.Where(x => libNames.Contains(x.Name)))
			{
				if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();
				
				string localEntryDest = Path.Combine(CLibFolder, entry.Name);
				Console.Write($"Expand {entry} -> {localEntryDest}... ");
				entry.ExtractToFile(localEntryDest, overwrite: true);
			}
		}
		else if (Directory.Exists(url))
		{
			foreach (string libName in libNames.All)
			{
				if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();
				
				string srcFile = Path.Combine(url, libName);
				string destFile = Path.Combine(CLibFolder, libName);
				if (!File.Exists(srcFile) && libName != libNames.Primary)
				{
					Console.WriteLine($"{srcFile} not exists, skip... ");
				}
				else
				{
					Console.WriteLine($"Copy {srcFile} to {destFile}... ");
					File.Copy(srcFile, destFile, overwrite: true);
				}
			}
		}
		Console.WriteLine("Done");
	}
}

public abstract record NupkgBuildSource(string rid, string titleRid, LibNames libNames, string url, string pkgName, string[] deps)
{
	public string CLibFilePath => Path.Combine(CLibFolder, libNames.Primary);
	public string CLibFolder => Path.Combine(RidFolder, "bin");
	public string RidFolder => $@"{titleRid}-{Path.GetFileNameWithoutExtension(libNames.Primary)}";
	public string Version => Projects.First(x => x.name == pkgName).version;
	public string NuGetPath => Path.Combine("nupkgs", $"{pkgName}.runtime.{titleRid}.{Version}.nupkg");

	protected abstract Task Decompress(CancellationToken cancellationToken);
	protected string[] GetDlls()
	{
		return Directory.EnumerateFiles(CLibFolder)
			.Where(x => Path.GetExtension(x) switch
			{
				".dll" => true,
				_ => false,
			})
			.Select(f => f.Replace(rid + @"\", ""))
			.ToArray();
	}

	public async Task Process(CancellationToken cancellationToken)
	{
		Console.WriteLine($"processing {titleRid}...");
		if (File.Exists(CLibFilePath))
		{
			Console.WriteLine($"{CLibFilePath} exists, override!");
		}
		await Decompress(cancellationToken);

		string[] libs = GetDlls();

		string nugetExePath = await EnsureNugetExe(cancellationToken);

		Directory.CreateDirectory(Path.GetDirectoryName(CLibFilePath)!);
		string nuspecPath = BuildNuspec(libs, rid, titleRid, RidFolder, pkgName, deps);
		if (File.Exists(NuGetPath))
		{
			Console.WriteLine($"{NuGetPath} exists, override!");
		}

		NuGetRun($@"pack {nuspecPath} -Version {Version} -OutputDirectory .\nupkgs".Dump());
	}
}

public class LibNames
{
	public string Primary { get; }
	public IReadOnlyCollection<string> All { get; }

	public LibNames(params string[] libNames)
	{
		if (libNames.Length == 0) throw new ArgumentException(nameof(libNames));

		Primary = libNames[0];
		All = libNames.ToHashSet();
	}

	public bool Contains(string name) => All.Contains(name);
}