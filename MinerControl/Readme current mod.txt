Test config files are for GTX1070 and R9-390.
All miners in assemply are public and were taken from this forun.

Here is the list of changes.

Application’s launch.
All API requests were rewritten by NewtonsoftJson, so you have to extract NewtonsoftJson.dll to the folder with “MinerControl.exe” to calculate the prices correctly.
Following files are necessary for correct start of the application: MinerControl.exe, NewtonsoftJson.dll, Minercontrol.cfg.
Miners can be kept either in the same folder in subfolders, or separately in any place you like. In the second case path should be entered in “minerfolderpath” key (description is below).

Interface Section
1.   Added column “Pending”. Shows amount of pending coins.
2.   Added column “Balance BTC”. If pool’s API reports exchange rate, “Balance” values will be exchanged.
3.   Added “MU” column for measuring hashrate scales. As soon as different pools have different scales for common algos,  all hashrates in configs were normalized to “kH/s”.
4.   Added “CoinName” column. For particular coin mining.
5.   Added “AcSpWrk” column. Shows speed for the worker in config. If you have more than one rig, you can monitor every single part in common result. And "TopAvgSp" column for calculation average Worker speed. 
6.   Added “Config file” button. Now it is possible to switch configs “on-the-fly” from the library.  Application starts with MinerConfig.cfg by default as previously. It should still be in the launch directory.
7.   Added "IdleMining" checkbox. Enables automode, when "minidleseconds" value from config passed after user last input action.

"General" Section
1. All previous keys and options in this segment are working as they were.
2. Added key "minerfolderpath", because most of miners use "usewindow" option, that requires absolute path to be launched correctly. 
It can have 3 options:
- Full absolute path to folder with miners, for example "C:\\Miners\\Nvidia";
- Value "MinerControl". Than it gets full path to directory which contains "MinerControl.exe";
- Be empty, as in previous versions. In this case path remains relative.
The structure for launch path is: MinerFolderPath \APARAM1 \ APARAM2

3.Added WTM Buffer. Radically reduces amount of API requests and accelerates refresh speed.
 It reads the prices from WhatToMine with adjustable frequency. New keys in "general" block:

        "wtmsyncperiod": 3,       time in minutes to refresh WTM prices, by default 3 minutes
        "wtmbuffer": true,        enables or disables WTM Buffer. By default enabled.
        "wtmextracoins":true,     enables prices for coins not included in basic list of WTM. By default enabled. If disabled, prices for coins with "PriceID" in algo section will not be accuired.

4. Donations. Values can be managed by «donationpercentage" and «donationfrequency" keys  in config files. By default it is 0.5% and 60 mins  (18 seconds of mining per hour). You can either decrease, or increase values, if you like this version. It will help for further development and bugs fixing.

5. added key "minidleseconds" for managing idlemining start delay.
6.  added key "hidecolumngr1" for "general" section, hides "Price", "PowerCost", "Fees" columns, 
          and "hidecolumngr2" for "general" section, hides "RejectSpeed", "TopAvgSp", "AcSpWrk", "MU", "PoolFee" columns.

"Algorithms" section
1.  All previous keys and options in this segment are working as they were.
2.  Added “MU” field for measuring scales. As soon as different pools have different scales for common algos,  all hashrates in configs were normalized to “kH/s”.
3.  Added “DevFee” option for cases, when you use miners with devfees. They affect the values in column “Fees”.


 “Services” ("Pools") section 
Services LTCRabbitService, WePayBTC, Manual Service were not changed at all.
Common updates in this segment:
1.   Added “fee” option. Represents the pool fee value.
2.   Added “cweight” option. It is a custom multiplier,  that locally affects the price. For example, when price scale is incorrect, it can be corrected with “cweight” multiplier. 
3.   For some pools added “tag” and “cname” (coin name) keys. They are necessary for catching API’s data correctly, don’t change them.  If you want to add new strings to config for such pools, “tag” and “cname” should 1:1 comply with the spelling, returned by API.
4.   For pools with direct mining to wallets common account name "multi" is used. Particular coin wallets should be mentioned with "wallet" key in coin definition section.
5.   Added "priceid" key. Gets the profitability from WhaToMine foê definite coin. Should be equal to CoinId from WTM.
6.   Added "active" key. If false, the line in config will be skipped.
7.  For pools with self-defined profitabillity added possibility to switch to WTM-based. New key in "pool" section ("pricewtm": true) by default is false.
8.  Additional key in "pools" section "avgspeedticks":10  - number of ticks for averaging worker speed value. 1 tick happens in 1 minute. Values can be from 1 to 60, if 0 then disabled. By default 10 ticks.
9. Additional key "poolenabled" for "pool" section. Enables/disables complete pool.


“Nicehash” service.
-   Added all currently listed algos and PriceIds.
-   Profitability is calculated by NH API.
-   Detectstratum is replaced by “detectlocation” as far as it doesn’t work correctly with ethereum and zcash stratums. If enabled, should be ALWAYS “usa.nicehash.com” in config file. It detects the best location on the bases of fastest ping. And replaces the “usa” to the closest location. Also helpful when servers in region are down.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ), to pools API. By default is disabled.

“Nanopool” service.
-   Added all currently listed coins. XMR support will be added soon, no API  is active yet.
-   Profitability is calculated by pools API.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ),  to pools API. By default is disabled.

“YAAMP-Clones” service.
-   Added all currently listed algos.
-   Profitability is calculated by pools API.
-   PriceMode selection:  (0) for “estimate_current”,  (1) for “estimate_last24h”,  (2) for “actual_last24h".
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ),  to pools API. By default is disabled.
-   Pool fees are taken from Pool API.

“MiningPoolHub”.
-   Added all currently listed coins.
-   Profitability is calculated by pools API.
-   “Account”, “ApiKey”, “UserID” are requested for correct API Data.
-   "Balancemode" selection: (0) reports “Credited”, (1) reports credited for AutoExchange, (2) reports “On Exchange”.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ),  to pools API. By default is disabled.
-   “Autofee” option. When true gets fees values from pools API. If entered manually in config, will be overwritten. By default is disabled.

“WhatToMine”.
Added all currently listed in API coins.
-   Profitability is calculated by current nethash, blocktime, blockrewards values.
-   “Pricemode” selection: (0) current values; (1) average 24h values.
-   Can be attached to any pool you like.
-   No balance and speed.
-   Completely free-of charge. Even in donation mode mining keeps going to your accounts.
-   To be recognized correctly you have to write “WTM-“ before the coins name in service title, i.e. “WTM-Decred”, “WTM-Ethereum” and so on (see in configs sample)

 “CoinMine.pl”&”Suprnova.cc”&"Miningfield.com&AikaPool".
-   Profitability is calculated by WhaToMine.
-   Only corresponding to WhatToMine coins can be added.
-   “Account”, “ApiKey”, “UserID” are requested for correct API Data.
-   “Pricemode” selection: (0) current values; (1) average 24h values.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ),  to pools API. By default is disabled.
-   “Autofee” option. When true gets fees values from pools API. If entered manually in config, will be overwritten. By default is disabled.

“Ethermine”&”Flypool”.
-   Profitability is calculated by WhatToMine.
-   Only corresponding to WhatToMine coins can be added.
-   Pricemode” selection: (0) current values; (1) average 24h values.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ), to pools API. By default is disabled.
-    API for ETC is unavailable from ethermine side.

“TheBlockFactory”.
-   Profitability is calculated by WhatToMine.
-   Only corresponding to WhatToMine coins can be added.
-   Pricemode” selection: (0) current values; (1) average 24h values.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ), to pools API. By default is disabled.

“GiveMeCoins”.
-   Profitability is calculated by WhatToMine.
-   Only corresponding to WhatToMine coins can be added.
-   Pricemode” selection: (0) current values; (1) average 24h values.
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ), to pools API. By default is disabled.

“Dwarfpool”.
-   Profitability is calculated by WhatToMine.
-   All pools for coins are added.
-   Pricemode” selection: (0) current values; (1) average 24h values.
-   Dwarfpool's API doesn't support balance reporting.
-   Only total hashrate speed and separate worker speed are available.

“Coinotron”.
-   Profitability is calculated by WhatToMine.
-   Only corresponding to WhatToMine coins can be added.
-   Pricemode” selection: (0) current values; (1) average 24h values.
-   Coinotrons's API doesn't support balance and speed reporting.

“YIMMP & Coin-Miners” service.
-   Added all currently listed algos.
-   Profitability is calculated by pools API.
-   PriceMode selection:  (0) for “estimate_current”,  (1) for “estimate_last24h”,  (2) for “actual_last24h".
-   “Nobalance” option. When true, switches off the requests for checking balances to pools API. By default is disabled.
-   “Nospeed” option. When true, switches off the requests for checking hashrate speed to pools API. By default is disabled.
-   “Nospeedworker” option. When true, switches off the requests for checking particular worker speed (mentioned in “worker” string ),  to pools API. By default is disabled.
-   Pool fees are taken from Pool API.

“DualMining” for Claymore miner.
-   Profitability is calculated by WhatToMine.
-   All dual coins are supported.
-   Pricemode” selection: (0) current values; (1) average 24h values.
-   accounts or direct wallets can be added to string parameters directly ("arguments" section).
-   No balance and speed reported. If you need it, add strings for designated pools with mono coins.



