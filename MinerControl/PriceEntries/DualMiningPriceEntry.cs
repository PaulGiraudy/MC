namespace MinerControl.PriceEntries
{
    public class DualMiningPriceEntry : PriceEntryBase
    {

        public string CoinNameFirst { get; set; }
        public string CoinNameSecond { get; set; }
        public decimal HashrateFirst { get; set; }
        public decimal HashrateSecond { get; set; }

        public string TagFirst { get; set; }
        public string TagSecond { get; set; }
        public decimal PriceFirst { get; set; }
        public decimal PriceSecond { get; set; }

    }
}