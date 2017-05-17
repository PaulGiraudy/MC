﻿using System;
using System.Collections.Generic;
using System.Linq;
using MinerControl.PriceEntries;

namespace MinerControl.History
{
    public class ServiceHistory
    {
        public string Service { get; set; }

        private readonly TimeSpan _statWindow;
        private readonly double _percentile;
        private readonly decimal _iqrMultiplier;

        public Dictionary<PriceEntryBase, List<PriceStat>> PriceList { get; set; } // PriceEntry:list of stats
        public class PriceStat
        {
            public DateTime Time { get; set; }
            public decimal CurrentPrice { get; set; }

            public decimal WindowedAveragePrice { get; set; } // Uses statwindow time to get an average
            public decimal TotalAveragePrice { get; set; }
            public bool Outlier { get; set; }
        }

        public ServiceHistory(string service, TimeSpan window, double percentile, decimal iqrMultiplier)
        {
            Service = service;
            _statWindow = window;
            _percentile = percentile;
            _iqrMultiplier = iqrMultiplier;
            PriceList = new Dictionary<PriceEntryBase, List<PriceStat>>();
        }

        public void UpdatePrice(PriceEntryBase priceEntryBase)
        {
            DateTime now = DateTime.Now;
            decimal price = priceEntryBase.NetEarn;
            
            decimal totalPrice = price;
            int totalCount = PriceList.Count;
            
            List<decimal> window = new List<decimal> {price};
            decimal windowedPrice = price;
            int windowedCount = 0;
            bool outlier = false;

            if (!PriceList.ContainsKey(priceEntryBase))
                PriceList.Add(priceEntryBase, new List<PriceStat>());

            foreach (PriceStat stat in PriceList[priceEntryBase])
            {
                if (stat.CurrentPrice > 0)
                {
                    decimal historicPrice = stat.CurrentPrice;
                    totalPrice += historicPrice;

                    if (stat.Time == now) return;
                    if (stat.Time >= now - _statWindow)
                    {
                        window.Add(historicPrice);
                        windowedPrice += historicPrice;
                        windowedCount++;
                    }
                }
            }

            decimal totalAveragePrice = totalPrice/(totalCount+1);
            decimal windowedAveragePrice = windowedPrice/(windowedCount+1);

            if (windowedCount >= 10) // Makes sure at least ten entries are in there
            {
                window.Sort();
                if (_iqrMultiplier > 0)
                {
                    double sumOfSquareOfDifferences =
                        (double) PriceList[priceEntryBase].Where(stat => stat.Time >= now - _statWindow)
                            .Sum(stat =>
                                (stat.CurrentPrice - windowedAveragePrice) *
                                (stat.CurrentPrice - windowedAveragePrice));
                    decimal standardDeviation = (decimal) Math.Sqrt(sumOfSquareOfDifferences/windowedCount);
                    decimal top = windowedAveragePrice + standardDeviation;
                    decimal bottom = windowedAveragePrice - standardDeviation;
                    decimal iqr = top - bottom;
                    decimal max = windowedAveragePrice + (iqr * _iqrMultiplier);
                    outlier = price > max;
                }
                else
                {
                    // If the IQR multiplier is negative or zero, 
                    // it'll try to use percentiles for outlierdetection
                    // Not advised! Will kill off trending profits, iqr-multiplying is preferable 
                    int outlierIndex = (int) Math.Truncate(window.Count*_percentile);
                    decimal[] outliers = {window[outlierIndex], window[window.Count - outlierIndex]};
                    outlier = price > outliers.Max();
                }
            }


            PriceStat priceStat = new PriceStat
            {
                Time = now,
                CurrentPrice = price,
                TotalAveragePrice = totalAveragePrice,
                WindowedAveragePrice = windowedAveragePrice,
                Outlier = outlier
            };

            priceEntryBase.Outlier = outlier;
            priceEntryBase.NetAverage = windowedAveragePrice;

          // if (priceEntryBase.Enabled == true)
                PriceList[priceEntryBase].Add(priceStat);
        }
    }
}
