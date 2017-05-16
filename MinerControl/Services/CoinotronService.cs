using MinerControl.PriceEntries;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MinerControl.Services
{
    public class CoinotronService : ServiceBase<CoinotronPriceEntry>
    {
        public CoinotronService()
        {
            ServiceName = "Coinotron";
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
        }

    }
}