﻿namespace MinerControl
{
    public class AlgorithmEntry
    {
        public string Name { get; set; }
        public string Display { get; set; }
        public decimal Hashrate { get; set; }
        public string MU { get; set; }
        public decimal Power { get; set; }
        public decimal? DevFee { get; set; }
        public string Priority { get; set; }
        public int Affinity { get; set; }

        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }
}