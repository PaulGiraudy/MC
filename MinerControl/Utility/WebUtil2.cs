using System;
using System.Net;
using System.Text;
using System.Threading;
using MinerControl.Services;
using Newtonsoft.Json;

namespace MinerControl.Utility
{
    public static class WebUtil2
    {
        public static void DownloadJson(string url, Action<object> jsonProcessor)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri(url);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                  | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    client.Encoding = Encoding.UTF8;
                    client.DownloadStringCompleted += DownloadJsonComplete;
                    client.DownloadStringAsync(uri, jsonProcessor);
                    //Thread.Sleep(50);
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

        private static void DownloadJsonComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            Action<object> jsonProcessor = e.UserState as Action<object>;
            if (jsonProcessor != null)
            {
                try
                {
                    if (e.Error != null) return;
                    string pageString = e.Result;
                    if (string.IsNullOrEmpty(pageString) || pageString == "") return;
                    object data = JsonConvert.DeserializeObject(pageString);

                    jsonProcessor(data);
                }
                catch (Exception ex)
                {
                    IService service = jsonProcessor.Target as IService;
                    if (service != null && jsonProcessor.Method.Name == "ProcessPrices") service.UpdateHistory(true);
                    ErrorLogger.Log(ex);
                }
            }
        }
    }
}