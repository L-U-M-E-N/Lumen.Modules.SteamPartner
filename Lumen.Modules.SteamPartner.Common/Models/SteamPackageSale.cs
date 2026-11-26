namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamPackageSale {
        public Guid Id { get; set; } = Guid.NewGuid();
        public uint? PackageId { get; set; }
        public uint? BundleId { get; set; }
        public uint AppId { get; set; }
        public DateOnly Date { get; set; }
        public string? PackageSaleType { get; set; } = null!;
        public string? Platform { get; set; }
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public string CountryRegion { get; set; } = null!;
        public decimal? BasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Currency { get; set; }
        public int? GrossUnitsActivated { get; set; }
        public int? GrossUnitsSold { get; set; }
        public int? GrossUnitsReturned { get; set; }
        public int? NetUnitsSold { get; set; }
        public decimal? GrossSalesUsd { get; set; }
        public decimal? GrossReturnsUsd { get; set; }
        public decimal? NetTaxUsd { get; set; }
        public decimal? NetSalesUsd { get; set; }
        public int? AdditionalRevenueShareTier { get; set; }
        public int? CombinedDiscountId { get; set; }
        public decimal? AvgSalePriceUsd { get; set; }
        public uint? KeyRequestId { get; set; }
    }
}
