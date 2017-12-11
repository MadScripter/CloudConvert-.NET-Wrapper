<div align="center"><h2>CloudConvert API .NET Wrapper</h2></div>

<h6>Update [16/10/2013] - Added the methods to delete and cancel a conversion.</h6>
=========================
<h3><b>How to use:</b></h3>

1 - Add the following directive:<br>
```csharp
using MadScripterWrappers;
```

2 - Create an instance of the CloudConvert class:<br>
```csharp
CloudConvert api = new CloudConvert("YOUR API KEY");
```
3 - Create a new Process ID:<br>
```csharp
api.GetProcessURL("Input format", "Output format");
```
4 - Upload a file for conversion:<br>
```csharp
api.UploadFile("The process URL which was created in the previous step","Path to the file you want to upload",
"The output format to which the file will be converted to","The email that will be notified once the conversion is done","A Dictionary of options which are format specific");
```

A Dictionary would be something like this for an image format:
```csharp
Dictionary<string, string> options = new Dictionary<string, string>()
{
	{ "resize", "128x128" },
	{ "rotate", "90" }
}
```

<i><u>Note:</u> The email and options are optional parameters.<i> 

<div align="center"><u>Formats can be found here: https://cloudconvert.org/formats</u></div>
<h5>Getting the status of a conversion:</h5>

```csharp
api.GetStatus("The process URL which was created in step 3");
```

<h5>Listing the running conversions of an API key:</h5>
```csharp
api.ListProcesses();
```

For the options here's a list:
<pre>
<b>// Video - Audio formats</b>
video_codec
video_bitrate
video_resolution
video_ratio
video_fps
video_crf
video_qscale
		
audio_codec
audio_bitrate
audio_frequency
audio_normalize
audio_qscale
		
trim_from
trim_to

<b>// azw - azw3 format</b>
outputprofile
authors
title

<b>// Image formats</b>		
resize
resizemode
rotate
quality

<b>// djvu format</b>
density

<b>// odt format</b>	
page_range
password

<b>// dwg format</b>		
autocad_version

<b>// pdf to html</b>	
no_images

<b>// PSD format</b>	
mergelayers
</pre>

5 - Download completed file:<br>
```csharp
Crude method to download file, please review before using. This method will block until an error occurs or the file is downloaded and returned as a byte[].

//Usage
DownloadFile("The returned process URL from step 3", int pollingDelayMilliseconds, bool deleteAfterConvert);
//Example:
DownloadFile(myProcessURL, 1000, true);
```

<div align="center"><i><b>More information can be found here:</b></i> https://cloudconvert.org/page/api</div>
