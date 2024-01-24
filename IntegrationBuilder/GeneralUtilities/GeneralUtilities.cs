using System.IO.Compression;
using System.Net;
using System.Text;

namespace IntegrationBuilder.GeneralUtilities
{
    public class GeneralUtilities
    {
        public static bool SendRequest(string jsonData, string sUrl, out string sErrormessage, out string result)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(sUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 180000; //--> 3 min
            httpWebRequest.ReadWriteTimeout = 180000; //--> 3 min

            HttpWebResponse httpResponse;

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception exc)
            {
                sErrormessage = exc.Message;
                result = "";
                return false;
            }

            Stream responseStream = responseStream = httpResponse.GetResponseStream();
            if (httpResponse.ContentEncoding.ToLower().Contains("gzip"))
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (httpResponse.ContentEncoding.ToLower().Contains("deflate"))
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StreamReader Reader = new StreamReader(responseStream, Encoding.GetEncoding(1253));
            result = Reader.ReadToEnd();



            httpResponse.Close();
            responseStream.Close();
            sErrormessage = "";
            return true;
        }

        public static string GetCleanCodeFromHuggingChat(string initialcode,out string error) 
        {
            error = "";
            try
            {
                if (string.IsNullOrEmpty(initialcode))
                {
                    error = "Hugging Chat Response was emty";
                    return "";
                }
                else 
                {
                    if (initialcode.Contains("```csharp"))
                    {
                        int startIndex = initialcode.IndexOf("```csharp");
                        if (startIndex == -1) 
                        {
                            error = "Response was not in default format";
                            return initialcode;
                        }
                        int endIndex = initialcode.IndexOf("```", startIndex + 7);
                        if (endIndex == -1) 
                        {
                            error = "Response was not in default format";
                            return initialcode;
                        }
                        string cleanCode = initialcode.Substring(startIndex + 9, endIndex - startIndex - 9).Trim();
                        return cleanCode;

                    }
                    else 
                    {
                        error = "Response was not in default format";
                        return initialcode;
                    }
                }
            }
            catch (Exception exc) 
            {
                error = $"Exception in GetCleanCodeFromHuggingChat. Exception messaeg:{exc.Message}";
                return "";
            }
        }
    }
}
