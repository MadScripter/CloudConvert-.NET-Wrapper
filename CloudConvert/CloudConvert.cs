/*
 *  The MIT License (MIT)
 *  Copyright (c) 2013 MadScripter
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy of
 *	this software and associated documentation files (the "Software"), to deal in
 *	the Software without restriction, including without limitation the rights to
 *	use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 *	the Software, and to permit persons to whom the Software is furnished to do so,
 *	subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 *	FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 *	COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 *	IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 *	CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 *  http://madscripter.tumblr.com/
 *  https://cloudconvert.org/
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;

namespace MadScripterWrappers
{

	public class CloudConvert
	{
		
		private string _apikey;
		private string _url;
		
		/// <param name="apikey">The API key which can be found here: https://cloudconvert.org/user/profile</param>
		public CloudConvert(string apikey)
		{
			_apikey = apikey;
		}
		
		/// <summary>
		/// Creates a new Process ID.
		/// </summary>
		/// <param name="inputFormat">Current format of the file.</param>
		/// <param name="outputFormat">Output format, to which the file should be converted.</param>
		/// <returns>The URL to upload a file, check the status of a conversion and download the converted file.</returns>
		public string GetProcessURL(string inputFormat, string outputFormat)
		{
			try
			{
				using(WebClient client = new WebClient())
				{
					client.Headers["Content-Type"] = "application/www-x-form-urlencoded";
					
					ProcessURL data = new JavaScriptSerializer().Deserialize<ProcessURL>(client.DownloadString(string.Format("https://api.cloudconvert.org/process?inputformat={0}&outputformat={1}&apikey={2}",
					                                                                                                         inputFormat,
					                                                                                                         outputFormat,
					                                                                                                         _apikey)));
					_url = data.url;
					
					if(!_url.StartsWith("http"))
					{
						_url = _url.Insert(0, "https:");
					}
					
					return _url;
				}
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}
		
		/// <summary>
		/// Uploads a file for conversion.
		/// </summary>
		/// <param name="processUrl">The URL to upload a file which you get using the GetProcessURL method.</param>
		/// <param name="filePath">Path to the file which will be converted.</param>
		/// <param name="outputFormat">Output format, to which the file should be converted.</param>
		/// <param name="email">E-mail address which is notified after the conversion is finished.</param>
		/// <param name="options">Conversion type specific options.</param>
		/// <returns></returns>
		public string UploadFile(string processUrl, string filePath, string outputFormat, string email = null, Dictionary<string, string> options = null)
		{
			try
			{
				using(WebClient client = new WebClient())
				{
					byte[] result;
					string sOpt;
					
					client.Headers["Content-Type"] = "binary/octet-stream";
					
					StringBuilder sb = new StringBuilder();
					
					if(options != null)
					{
						foreach(KeyValuePair<string, string> kvp in options)
						{
							if(options.Count > 1)
							{
								sb.Append(string.Format("options[{0}]={1}&", kvp.Key, kvp.Value));
							}
							else
							{
								sb.Append(string.Format("options[{0}]={1}", kvp.Key, kvp.Value));
							}
						}
						
						if(sb.ToString().LastIndexOf('&') > 0)
						{
							sOpt = sb.ToString().Remove(sb.ToString().Length - 1, 1);
						}
						else
						{
							sOpt = sb.ToString();
						}

						result = client.UploadFile(string.Format("{0}?input=upload&format={1}&file={2}&email={3}&{4}",
						                                         processUrl,
						                                         outputFormat,
						                                         HttpUtility.UrlEncode(filePath),
						                                         email, sOpt),
						                           filePath);
					}
					else
					{
						result = client.UploadFile(string.Format("{0}?input=upload&format={1}&file={2}&email={3}",
						                                         processUrl,
						                                         outputFormat,
						                                         HttpUtility.UrlEncode(filePath),
						                                         email),
						                           filePath);
					}
					
					return Encoding.UTF8.GetString(result, 0, result.Length);

				}
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}
		
		/// <summary>
		/// Gets the status of a conversion.
		/// </summary>
		/// <param name="processUrl">The URL to upload a file which you get using the GetProcessURL method.</param>
		/// <returns></returns>
		public string GetStatus(string processUrl)
		{
			try
			{
				using(WebClient client = new WebClient())
				{
					client.Headers["Content-Type"] = "application/www-x-form-urlencoded";
					
					return client.DownloadString(processUrl);
				}
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}
		
		/// <summary>
		/// Lists all the running conversions.
		/// </summary>
		/// <returns></returns>
		public string ListProcesses()
		{
			try
			{
				using(WebClient client = new WebClient())
				{
					client.Headers["Content-Type"] = "application/www-x-form-urlencoded";
					
					return client.DownloadString(string.Format("https://api.cloudconvert.org/processes?apikey={0}", _apikey));
				}
			}
			catch(Exception e)
			{
				return e.Message;
			}
		}
	}
	
	public class ProcessURL
	{
		public string url { get; set; }
	}
	
	/// <summary>
	/// The class for deserializing the conversion status.
	/// </summary>
	public class ConversionStatus
	{
		public string id { get; set; }
		public string url { get; set; }
		public string percent { get; set; }
		public string message { get; set; }
		public string step { get; set; }
		public long starttime { get; set; }
		public long expire { get; set; }
		public Input input { get; set; }
		public string path { get; set; }
		public Converter converter { get; set; }
		public Output output { get; set; }
		public long endtime { get; set; }
	}
	
	public class Input
	{
		public string type { get; set; }
		public string filename { get; set; }
		public long size { get; set; }
		public string name { get; set; }
		public string ext { get; set; }
	}
	
	public class Converter
	{
		public string format { get; set; }
		public string type { get; set; }
		public Options options { get; set; }
		public double duration { get; set; }
		
	}
	
	public class Options
	{
		public string inputformat { get; set; }
		public string outputformat { get; set; }
		public string converter { get; set; }
		public ConverterOptions converteroptions { get; set; }
		public string note { get; set; }
		public string group { get; set; }
		
	}
	
	public class ConverterOptions
	{
		public string video_codec { get; set; }
		public string video_bitrate { get; set; }
		public string video_resolution { get; set; }
		public string video_ratio { get; set; }
		public string video_fps { get; set; }
		public string video_crf { get; set; }
		public string video_qscale { get; set; }
		
		public string audio_codec { get; set; }
		public string audio_bitrate { get; set; }
		public string audio_frequency { get; set; }
		public string audio_normalize { get; set; }
		public string audio_qscale { get; set; }
		
		public string trim_from { get; set; }
		public string trim_to { get; set; }
		
		public string outputprofile { get; set; }
		public string authors { get; set; }
		public string title { get; set; }
		
		public string resize { get; set; }
		public string resizemode { get; set; }
		public string rotate { get; set; }
		
		public string quality { get; set; }
		public string density { get; set; }
		
		public string page_range { get; set; }
		public string password { get; set; }
		
		public string autocad_version { get; set; }
		
		public string no_images { get; set; }
		
		public string mergelayers { get; set; }
		
	}
	
	public class Output
	{
		public string filename { get; set; }
		public long size { get; set; }
		public string url { get; set; }
		public int downloads { get; set; }
	}
	
	/// <summary>
	/// The class for deserializing the list of running processes.
	/// </summary>
	public class ListRC
	{
		public string id { get; set; }
		public string host { get; set; }
		public string step { get; set; }
		public string starttime { get; set; }
		public string endtime { get; set; }
		public string url { get; set; }
	}
	
}