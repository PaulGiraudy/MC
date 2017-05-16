using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;


namespace MinerControl.Utility
{
    public static class WebUtil3
    {
        public static object DownloadJson(string url)
        {
            object RawData = null;

            try
            {
                using (WebClient client = new WebClient())
                {
                    Uri uri = new Uri(url);
                    client.Encoding = Encoding.UTF8;

                    var JsonRaw = client.DownloadString(uri);
                    if (!string.IsNullOrEmpty(JsonRaw) && JsonRaw != "")
                        RawData = JsonConvert.DeserializeObject(JsonRaw);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }

            return RawData;
        }

    }
}
