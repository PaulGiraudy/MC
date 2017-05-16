using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class MiningfieldService : ServiceBase<MiningfieldPriceEntry>
    {
        public MiningfieldService()
        {
            ServiceName = "Miningfield";
        }

        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
        }

        public override void CheckFees()
        {
            if (_autofee == true)
                FeesUpdate("http://" + "TAG" + ".miningfield.com/index.php?page=api&action=getpoolinfo&api_key=" + "APIKEY");
        }
        public override void CheckPrices()
        {
            WTMUpdate();
        }
        public override void CheckData()
        {
            string urs = "http://" + "TAG" + ".miningfield.com/index.php?page=api&action=getdashboarddata&api_key=" + "APIKEY";
            string urb = "http://" + "TAG" + ".miningfield.com/index.php?page=api&action=getuserbalance&api_key=" + "APIKEY" + "&id=" + "USERID";
            string urw = "http://" + "TAG" + ".miningfield.com/index.php?page=api&action=getuserworkers&api_key=" + "APIKEY" + "&id=" + "USERID";

            MPOSDataUpdate(urs, urb, urw);
        }

    }
}



