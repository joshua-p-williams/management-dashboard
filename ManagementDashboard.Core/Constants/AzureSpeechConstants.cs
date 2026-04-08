using System;

namespace ManagementDashboard.Core.Constants
{
    public static class AzureSpeechConstants
    {
        public static class Regions
        {
            public const string EastUs = "eastus";
            public const string EastUs2 = "eastus2";
            public const string WestUs = "westus";
            public const string WestUs2 = "westus2";
            public const string WestUs3 = "westus3";
            public const string CentralUs = "centralus";
            public const string NorthCentralUs = "northcentralus";
            public const string SouthCentralUs = "southcentralus";
            public const string WestCentralUs = "westcentralus";
            public const string CanadaCentral = "canadacentral";
            public const string CanadaEast = "canadaeast";
            public const string BrazilSouth = "brazilsouth";
            public const string WestEurope = "westeurope";
            public const string NorthEurope = "northeurope";
            public const string UKSouth = "uksouth";
            public const string UKWest = "ukwest";
            public const string FranceCentral = "francecentral";
            public const string GermanyWestCentral = "germanywestcentral";
            public const string SwitzerlandNorth = "switzerlandnorth";
            public const string NorwayEast = "norwayeast";
            public const string SwedenCentral = "swedencentral";
            public const string SouthAfricaNorth = "southafricanorth";
            public const string AustraliaEast = "australiaeast";
            public const string AustraliaSoutheast = "australiasoutheast";
            public const string EastAsia = "eastasia";
            public const string SoutheastAsia = "southeastasia";
            public const string JapanEast = "japaneast";
            public const string JapanWest = "japanwest";
            public const string KoreaCentral = "koreacentral";
            public const string CentralIndia = "centralindia";
            public const string SouthIndia = "southindia";
            public const string WestIndia = "westindia";

            public static readonly Dictionary<string, string> RegionDisplayNames = new Dictionary<string, string>
            {
                { EastUs, "East US" },
                { EastUs2, "East US 2" },
                { WestUs, "West US" },
                { WestUs2, "West US 2" },
                { WestUs3, "West US 3" },
                { CentralUs, "Central US" },
                { NorthCentralUs, "North Central US" },
                { SouthCentralUs, "South Central US" },
                { WestCentralUs, "West Central US" },
                { CanadaCentral, "Canada Central" },
                { CanadaEast, "Canada East" },
                { BrazilSouth, "Brazil South" },
                { WestEurope, "West Europe" },
                { NorthEurope, "North Europe" },
                { UKSouth, "UK South" },
                { UKWest, "UK West" },
                { FranceCentral, "France Central" },
                { GermanyWestCentral, "Germany West Central" },
                { SwitzerlandNorth, "Switzerland North" },
                { NorwayEast, "Norway East" },
                { SwedenCentral, "Sweden Central" },
                { SouthAfricaNorth, "South Africa North" },
                { AustraliaEast, "Australia East" },
                { AustraliaSoutheast, "Australia Southeast" },
                { EastAsia, "East Asia" },
                { SoutheastAsia, "Southeast Asia" },
                { JapanEast, "Japan East" },
                { JapanWest, "Japan West" },
                { KoreaCentral, "Korea Central" },
                { CentralIndia, "Central India" },
                { SouthIndia, "South India" },
                { WestIndia, "West India" }
            };
        }

        public static class Languages
        {
            public const string EnglishUs = "en-US";
            public const string EnglishUk = "en-GB";
            public const string EnglishCanada = "en-CA";
            public const string EnglishAustralia = "en-AU";
            public const string EnglishIndia = "en-IN";
            public const string SpanishSpain = "es-ES";
            public const string SpanishMexico = "es-MX";
            public const string FrenchFrance = "fr-FR";
            public const string FrenchCanada = "fr-CA";
            public const string GermanGermany = "de-DE";
            public const string ItalianItaly = "it-IT";
            public const string PortugueseBrazil = "pt-BR";
            public const string PortuguesePortugal = "pt-PT";
            public const string JapaneseJapan = "ja-JP";
            public const string KoreanKorea = "ko-KR";
            public const string ChineseSimplified = "zh-CN";
            public const string ChineseTraditional = "zh-TW";

            public static readonly Dictionary<string, string> LanguageDisplayNames = new Dictionary<string, string>
            {
                { EnglishUs, "English (United States)" },
                { EnglishUk, "English (United Kingdom)" },
                { EnglishCanada, "English (Canada)" },
                { EnglishAustralia, "English (Australia)" },
                { EnglishIndia, "English (India)" },
                { SpanishSpain, "Spanish (Spain)" },
                { SpanishMexico, "Spanish (Mexico)" },
                { FrenchFrance, "French (France)" },
                { FrenchCanada, "French (Canada)" },
                { GermanGermany, "German (Germany)" },
                { ItalianItaly, "Italian (Italy)" },
                { PortugueseBrazil, "Portuguese (Brazil)" },
                { PortuguesePortugal, "Portuguese (Portugal)" },
                { JapaneseJapan, "Japanese (Japan)" },
                { KoreanKorea, "Korean (Korea)" },
                { ChineseSimplified, "Chinese (Simplified)" },
                { ChineseTraditional, "Chinese (Traditional)" }
            };
        }

        public const string DefaultRegion = Regions.EastUs;
        public const string DefaultLanguage = Languages.EnglishUs;
        public const int DefaultMaxRecordingDurationMinutes = 5;
        public const int MinRecordingDurationMinutes = 1;
        public const int MaxRecordingDurationMinutes = 30;
    }
}