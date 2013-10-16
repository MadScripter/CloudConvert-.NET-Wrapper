<div align="center"><h2>CloudConvert API .NET Wrapper</h2></div>

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

<h5>Getting the status of a conversion:</h5>

```csharp
api.GetStatus("The process URL which was created in step 3");
```

<h5>Listing the running conversions of an API key:</h5>
```csharp
api.ListProcesses();
```

<div align="center"><i><b>More information can be found here:</b></i> https://cloudconvert.org/page/api</div>
