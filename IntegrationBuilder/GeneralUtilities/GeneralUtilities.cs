using System.IO.Compression;
using System.Net;
using System.Text;

namespace IntegrationBuilder.GeneralUtilities
{
    internal class GeneralUtilities
    {
        public static bool SendRequest(string jsonData, string sUrl, out string sErrormessage, out string result)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(sUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 10000;
            httpWebRequest.ReadWriteTimeout = 10000;

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
    }
}
