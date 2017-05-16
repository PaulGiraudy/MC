using MinerControl.PriceEntries;
using MinerControl.Utility;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class GiveMeCoinsService : ServiceBase<GiveMeCoinsPriceEntry>
    {
        public GiveMeCoinsService()
        {
            ServiceName = "GiveMeCoins";
        }
        public override void Initialize(IDictionary<string, object> data)
        {
            ReadData(data);
        }

        public override void CheckFees()
        {
        }
        public override void CheckPrices()
        {
            WTMUpdate();           
        }
            
        public override void CheckData()
        {
            string urd = "https://give-me-coins.com/pool/api-" + "TAG" + "?api_key=" + "APIKEY";
            APIDataUpdate(urd);
        }      
    }
}