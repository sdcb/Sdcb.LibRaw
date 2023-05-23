# Low level apis:

## LibRaw C API mapping:
Mapped to class: `LibRawNative`:

| C Function Name               | C# Function Name            |
| ----------------------------- | --------------------------- |
| libraw_strerror               | GetErrorMessage             |
| libraw_strprogress            | GetProgressMessage          |
| libraw_init                   | Initialize                  |
| libraw_open_file              | OpenFile                    |
| libraw_open_wfile             | OpenWideFile                |
| libraw_open_buffer            | OpenBuffer                  |
| libraw_open_bayer             | OpenBayerData               |
| libraw_unpack                 | Unpack                      |
| libraw_unpack_thumb           | UnpackThumbnail             |
| libraw_unpack_thumb_ex        | UnpackThumbnailExtended     |
| libraw_recycle_datastream     | RecycleDataStream           |
| libraw_recycle                | Recycle                     |
| libraw_close                  | Close                       |
| libraw_subtract_black         | SubtractBlack               |
| libraw_raw2image              | RawToImage                  |
| libraw_free_image             | FreeImage                   |
| libraw_version                | GetVersion                  |
| libraw_versionNumber          | GetVersionNumber            |
| libraw_cameraList             | GetCameraList               |
| libraw_cameraCount            | GetCameraCount              |
| libraw_set_exifparser_handler | SetExifParserHandler        |
| libraw_set_dataerror_handler  | SetDataErrorHandler         |
| libraw_set_progress_handler   | SetProgressHandler          |
| libraw_unpack_function_name   | GetUnpackFunctionName       |
| libraw_get_decoder_info       | GetDecoderInfo              |
| libraw_COLOR                  | GetColor                    |
| libraw_capabilities           | GetCapabilities             |
| libraw_adjust_sizes_info_only | AdjustSizesInfoOnly         |
| libraw_dcraw_ppm_tiff_writer  | WriteDcrawPpmTiff           |
| libraw_dcraw_thumb_writer     | WriteDcrawThumbnail         |
| libraw_dcraw_process          | ProcessDcraw                |
| libraw_dcraw_make_mem_image   | MakeDcrawMemoryImage        |
| libraw_dcraw_make_mem_thumb   | MakeDcrawMemoryThumbnail    |
| libraw_dcraw_clear_mem        | ClearDcrawMemory            |
| libraw_set_demosaic           | SetDemosaicAlgorithm        |
| libraw_set_output_color       | SetOutputColorSpace         |
| libraw_set_adjust_maximum_thr | SetAdjustMaximumThreshold   |
| libraw_set_user_mul           | SetUserMultiplier           |
| libraw_set_output_bps         | SetOutputBitsPerSample      |
| libraw_set_gamma              | SetGamma                    |
| libraw_set_no_auto_bright     | SetAutoBrightnessCorrection |
| libraw_set_bright             | SetBrightness               |
| libraw_set_highlight          | SetHighlightMode            |
| libraw_set_fbdd_noiserd       | SetNoiseReductionMode       |
| libraw_get_raw_height         | GetRawImageHeight           |
| libraw_get_raw_width          | GetRawImageWidth            |
| libraw_get_iheight            | GetProcessedImageHeight     |
| libraw_get_iwidth             | GetProcessedImageWidth      |
| libraw_get_cam_mul            | GetCameraMultiplier         |
| libraw_get_pre_mul            | GetPreMultiplier            |
| libraw_get_rgb_cam            | GetRgbCameraMatrix          |
| libraw_get_color_maximum      | GetColorMaximum             |
| libraw_set_output_tif         | SetOutputTiff               |
| libraw_get_iparams            | GetImageParameters          |
| libraw_get_lensinfo           | GetLensInformation          |
| libraw_get_imgother           | GetImageData                |
