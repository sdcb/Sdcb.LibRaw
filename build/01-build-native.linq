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
	await new WindowsNugetSource("win-x64", "win64", "gmp-10.dll", @"C:\_\3rd\vcpkg\packages\gmp_x64-windows\bin", "Sdcb.Arithmetic.Gmp", deps: new string[0])
		.Process(QueryCancelToken);
	await new WindowsNugetSource("win-x86", "win32", "gmp-10.dll", @"C:\_\3rd\vcpkg\packages\gmp_x86-windows\bin", "Sdcb.Arithmetic.Gmp", deps: new string[0])
		.Process(QueryCancelToken);
}

static string BuildNuspec(string[] libs, string rid, string titleRid, string folder, string pkgName, string[] deps)
{
	// props
	{
		string propsFile = $"./{pkgName}.runtime.props";
		XDocument props = XDocument.Parse(File.ReadAllText(propsFile));
		string normalizedName = pkgName.Replace(".", "") + "Dlls";
		string ns = props.Root.GetDefaultNamespace().NamespaceName;
		XmlNamespaceManager nsr = new(new NameTable());
		nsr.AddNamespace("p", ns);

		string platform = rid.Split("-").Last();

		XElement? itemGroup = props.XPathSelectElement("/p:Project/p:ItemGroup", nsr);
		if (itemGroup == null) throw new Exception($"{nameof(itemGroup)} invalid in {propsFile}");
		itemGroup.Add(
		libs
			.Select(x => Path.GetFileName(x))
			.Select(x => 
				new XElement(XName.Get("Content", ns), new XAttribute("Include", $@"$({normalizedName})\{rid}\native\{x}"),
					new XElement(XName.Get("Link", ns), @$"dll\{platform}\{x}"),
					new XElement(XName.Get("CopyToOutputDirectory", ns), "PreserveNewest")))
				);

		XElement? propGroup = props.XPathSelectElement("/p:Project/p:PropertyGroup", nsr);
		if (propGroup == null) throw new Exception($"{nameof(propGroup)} invalid in {propsFile}");
		propGroup.Add(new XElement(XName.Get(normalizedName, ns), @"$(MSBuildThisFileDirectory)..\..\runtimes"));
		props.Save(Path.Combine(folder, $"{titleRid}.props"));
	}

	// nuspec
	{
		string nuspecFile = $"./{pkgName}.runtime.nuspec";
		XDocument nuspec = XDocument.Parse(File
			.ReadAllText(nuspecFile)
			.Replace("{rid}", rid)
			.Replace("{pkgName}", pkgName)
			.Replace("{titleRid}", titleRid));

		string ns = nuspec.Root.GetDefaultNamespace().NamespaceName;
		XmlNamespaceManager nsr = new(new NameTable());
		nsr.AddNamespace("p", ns);

		XElement? files = nuspec.XPathSelectElement("/p:package/p:files", nsr);
		if (files == null) throw new Exception($"{nameof(files)} invalid in {nuspecFile}");
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
			if (dependencies == null) throw new Exception($"{nameof(dependencies)} invalid in {nuspecFile}");
			dependencies.Add(deps.Select(depId => new XElement(XName.Get("dependency", ns), 
				new XAttribute("id", $"{depId}.runtime.{titleRid}"), 
				new XAttribute("version", Projects.First(x => x.name == depId).version))));
		}

		string destinationFile = Path.Combine(folder, $"{titleRid}.nuspec");
		nuspec.Save(destinationFile);
		return destinationFile;
	}
}

public record WindowsNugetSource(string rid, string titleRid, string libName, string folder, string pkgName, string[] deps) : NupkgBuildSource(rid, titleRid, libName, folder, pkgName, deps)
{
	protected override Task Decompress(string folder, CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(CLibFolder);
		foreach (string entry in Directory.EnumerateFiles(folder).Where(x => Path.GetFileName(x) == libName))
		{
			string localEntryDest = Path.Combine(CLibFolder, Path.GetFileName(entry));
			Console.Write($"Expand {entry} -> {localEntryDest}... ");
			File.Copy(entry, localEntryDest, overwrite: true);
			Console.WriteLine("Done");
		}
		return Task.CompletedTask;
	}

	protected override string[] GetDlls()
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
}

public abstract record NupkgBuildSource(string rid, string titleRid, string libName, string folder, string pkgName, string[] deps)
{
	public string CLibFilePath => Path.Combine(CLibFolder, libName);
	public string CLibFolder => Path.Combine(RidFolder, "bin");
	public string RidFolder => $@"{titleRid}-{Path.GetFileNameWithoutExtension(libName)}";
	public string Version => Projects.First(x => x.name == pkgName).version;
	public string NuGetPath => Path.Combine("nupkgs", $"{pkgName}.runtime.{titleRid}.{Version}.nupkg");

	protected abstract Task Decompress(string localZipFile, CancellationToken cancellationToken);
	protected abstract string[] GetDlls();

	public async Task Process(CancellationToken cancellationToken)
	{
		Console.WriteLine($"processing {titleRid}...");
		if (File.Exists(CLibFilePath))
		{
			Console.WriteLine($"{CLibFilePath} exists, override!");
		}
		await Decompress(folder, cancellationToken);
		
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