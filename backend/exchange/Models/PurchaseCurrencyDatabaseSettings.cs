using System;
namespace exchange.Models
{
    /// <summary>
    ///  This class is used to store the appsettings.json file's. The JSON and C# property names are named identically to ease the mapping process.
    /// </summary>
    public class PurchaseCurrencyDatabaseSettings : IPurchaseCurrencyDatabaseSettings
    {
        public string PurchasesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPurchaseCurrencyDatabaseSettings
    {
        string PurchasesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}