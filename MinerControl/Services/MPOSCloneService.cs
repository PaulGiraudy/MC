using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class MPOSCloneService : ServiceBase<MPOSClonePriceEntry>
    {
        private string _urs;
        private string _urw;
        private string _urb;
        private string _urf;
            public MPOSCloneService(string pool)
        {
            ServiceName = pool.Remove ("MPOS-");
        }

        public override void Initialize(IDictionary<string, object> data)
        {
            if (data.ContainsKey("urlbalance")) _urb = data.GetString("urlbalance");
            else _urb = "https://" + ServiceName +"/index.php?page=api&action=getuserbalance&api_key=" + "APIKEY" + "&id=" + "USERID";

            if (data.ContainsKey("urlspeed")) _urs = data.GetString("urlspeed");
            else _urs = "https://" + ServiceName +"/index.php?page=api&action=getdashboarddata&api_key=" + "APIKEY"; 

            if (data.ContainsKey("urlworker")) _urw = data.GetString("urlworker");
            else _urw = "https://" + ServiceName +"/index.php?page=api&action=getuserworkers&api_key=" + "APIKEY" + "&id=" + "USERID";

            if (data.ContainsKey("urlfee")) _urf = data.GetString("urlfee");
            else _urf = "https://" + ServiceName + "/index.php?page=api&action=getpoolinfo&api_key=" + "APIKEY";

            ReadData(data);
        }

        public override void CheckFees()
        {
            if (_autofee == true)
                FeesUpdate(_urf);
        }

        public override void CheckPrices()
        {
            WTMUpdate();
        }

        public override void CheckData()
        {
             MPOSDataUpdate(_urs, _urb, _urw);
        }
    }
}



