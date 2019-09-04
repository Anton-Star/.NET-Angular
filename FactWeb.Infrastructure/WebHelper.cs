using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace FactWeb.Infrastructure
{
    public static class WebHelper
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// Gets the HTML from a given web page
        /// </summary>
        /// <param name="url">Url of the web page</param>
        /// <returns>String of the Html</returns>
        public static string GetHtml(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var data = string.Empty;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();

                if (receiveStream == null) return data;

                var readStream = response.CharacterSet == null ? new StreamReader(receiveStream) : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            return data;
        }

        public static MemoryStream Download(string url)
        {
            var webClient = new WebClient();
            var stream = new MemoryStream(webClient.DownloadData(url));

            return stream;
        }

        public static T Post<T>(string url, string parameters, string authType, string contentType, string apiKey)
        {
            using (var client = new WebClient())
            {
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.Headers[HttpRequestHeader.Authorization] = $"{authType} {apiKey}";
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    client.Headers[HttpRequestHeader.ContentType] = contentType;
                }

                var response = client.UploadString(url, parameters);

                return JsonConvert.DeserializeObject<T>(response);
            }
        }

        public static T Get<T>(string url, string apiKey)
        {
            return Get<T>(url, "Basic", apiKey);
   ;     }

        public static T Get<T>(string url, string authType, string apiKey)
        {
            using (var client = new WebClient())
            {
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.Headers[HttpRequestHeader.Authorization] = $"{authType} {apiKey}";
                }

                var response = client.DownloadData(url);
                var str = Encoding.UTF8.GetString(response);

                return JsonConvert.DeserializeObject<T>(str);
            }
        }

        public static T Delete<T>(string url, string apiKey)
        {
            var request = WebRequest.Create(url);
            request.Method = "DELETE";
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                request.Headers[HttpRequestHeader.Authorization] = "Basic " + apiKey;
            }

            using (var response = (HttpWebResponse) request.GetResponse())
            {
                var stream = response.GetResponseStream();
                var read = new StreamReader(stream);
                var data = read.ReadToEnd();

                return JsonConvert.DeserializeObject<T>(data);
            }
        }

        public static T Put<T>(string url, Dictionary<string, object> postParameters, string apiKey)
        {
            var formDataBoundary = "----WebKitFormBoundaryiaZECtmgkFfNucSL";
            var contentType = "multipart/form-data; boundary=" + formDataBoundary;

            var formData = GetMultipartFormData(postParameters, formDataBoundary);

            var data = PostForm(url, contentType, "PUT", formData, apiKey);

            return JsonConvert.DeserializeObject<T>(data);
        }

        public static T MultipartFormDataPost<T>(string postUrl, Dictionary<string, object> postParameters, string apiKey)
        {
            var formDataBoundary = "----WebKitFormBoundaryiaZECtmgkFfNucSL";
            var contentType = "multipart/form-data; boundary=" + formDataBoundary;

            var formData = GetMultipartFormData(postParameters, formDataBoundary);

            var data = PostForm(postUrl, contentType, "POST", formData, apiKey);

            return JsonConvert.DeserializeObject<T>(data);
        }
        public static string PostForm(string postUrl, string contentType, string method, byte[] formData, string apiKey)
        {
            return PostForm(postUrl, contentType, "Basic", method, formData, apiKey);
        }

        public static string PostForm(string postUrl, string contentType, string authType, string method, byte[] formData, string apiKey)
        {

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = contentType;

                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    client.Headers[HttpRequestHeader.Authorization] = $"{authType} {apiKey}";
                }

                var response = client.UploadData(postUrl, method, formData);
                return Encoding.UTF8.GetString(response);
            }
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            var formDataStream = new MemoryStream();
            var needsClrf = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsClrf)
                    formDataStream.Write(Encoding.GetBytes("\r\n"), 0, Encoding.GetByteCount("\r\n"));

                needsClrf = true;

                if (param.Value is FileParameter)
                {
                    var fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    var header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(Encoding.GetBytes(header), 0, Encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    var postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(Encoding.GetBytes(postData), 0, Encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            var footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(Encoding.GetBytes(footer), 0, Encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            var formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                this.File = file;
                this.FileName = filename;
                this.ContentType = contenttype;
            }
        }
    }
}
