using KarafkaConsumer_POC.Model;
using MemberProducerSync.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace MemberConsumerSync.Utils
{
    public static class HttpHelper
    {
        //public static void SendNextMessage(MemberModel member)
        //{
        //    var url = $"{ConfigHelper.Configuration.GetValue<string>("SyncMembers:url")}MemberNext";
        //    var message = JsonConvert.SerializeObject(member);

        //    SendMessage(url, message);
        //}

        public static void SendLegacyMessage(MemberModel member)
        {
            var url = "http://localhost:55292/api/MemberLegacy";
            member.RequestDate = DateTime.Now;
            member.Date = DateTime.Now;
            member.Code = string.Empty;
            var message = JsonConvert.SerializeObject(member);

            SendMessage(url, message);
        }

        public static void SendEventMember(MemberModel member)
        {
            var url = $"{ConfigHelper.Configuration.GetValue<string>("SyncMembers:url")}MemberSync";
            member.RequestDate = DateTime.Now;
            var message = JsonConvert.SerializeObject(member);

            SendMessage(url, message);
        }

        private static void SendMessage(string url, string message)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(message);

                streamWriter.Write(json);
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