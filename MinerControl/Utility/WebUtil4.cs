using System;
using System.Net;
using System.Text;
using MinerControl.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinerControl.Utility
{
    public static class WebUtil4
    {


        public static void DownloadJson(string entry, string url, Action<object> jsonProcessor)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri(url);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    client.Encoding = Encoding.UTF8;
                    client.DownloadStringCompleted += (s, e) =>
                    {
                        jsonProcessor = e.UserState as Action<object>;
                        if (jsonProcessor != null)
                        {
                            try
                            {
                                string pageString = e.Result;
                                if (string.IsNullOrEmpty(pageString) || pageString == "") return;
                                object data = JsonConvert.DeserializeObject(pageString);
                                JObject raw = (JObject)data;

                                JToken st = raw.First;
                                JToken fi = st.First;
                                //JToken status = raw["status"];
                                fi.Replace(entry);
                                data = raw.ToString();

                                jsonProcessor(raw);
                            }
                            catch (Exception ex)
                            {
                                IService service = jsonProcessor.Target as IService;
                                if (service != null && jsonProcessor.Method.Name == "ProcessPrices") service.UpdateHistory(true);
                                ErrorLogger.Log(ex);
                            }
                        }
                    };

                    client.DownloadStringAsync(uri, jsonProcessor);
                }
            }
            catch (Exception ex)
            {
                IService service = jsonProcessor.Target as IService;
                if (service != null && jsonProcessor.Method.Name == "ProcessPrices") service.UpdateHistory(true);
                // Makes sure the service is updated if price retrieval errors out
                ErrorLogger.Log(ex);
            }
        }


    }
}