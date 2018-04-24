using KarafkaConsumer_POC.Model;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace MemberConsumerSync.Utils
{
    public static class HttpHelper
    {
        public static void SendLegacyMessage(MemberModel member)
        {
            var msg = JsonConvert.SerializeObject(member);
            var url = "http://localhost:55292/api/MemberLegacy";

            SendMessage(url, msg);
        }

        public static void SendNextMessage(MemberModel member)
        {
            var msg = JsonConvert.SerializeObject(member);
            var url = "http://localhost:55292/api/MemberNext";

            SendMessage(url, msg);
        }

        private static void SendMessage(string url, string message)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(message);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}