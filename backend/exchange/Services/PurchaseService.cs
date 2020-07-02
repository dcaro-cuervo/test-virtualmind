using System;
using System.Collections.Generic;
using exchange.Models;
using MongoDB.Driver;

namespace exchange.Services
{
    public class PurchaseService
    {
        private readonly IMongoCollection<Purchase> _purchases;

        public PurchaseService(IPurchaseCurrencyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _purchases = database.GetCollection<Purchase>(settings.PurchasesCollectionName);
        }

        /// <summary>
        /// Gets purchase for this user in this month with this currency.
        /// </summary>
        /// <returns>Purchase by user identifier and month and currency.</returns>
        /// <param name="purchase">Purchase.</param>
        public List<Purchase> GetByUserIdAndMonthAndCurrency(Purchase purchase) =>
            _purchases.Find(x => x.UserId == purchase.UserId && x.Month == DateTime.Today.Month && x.Year == DateTime.Today.Year && x.Currency == purchase.Currency).ToList();

        public Purchase Create(Purchase purchase)
        {
            _purchases.InsertOne(purchase);

            return purchase;
        }

        public Purchase Get(string id) =>
            _purchases.Find(purchase => purchase.Id == id).FirstOrDefault();

        public void Update(string id, Purchase purchaseId) =>
            _purchases.ReplaceOne(purchase => purchase.Id == id, purchaseId);

        public void Remove(Purchase purchaseId) =>
            _purchases.DeleteOne(purchase => purchase.Id == purchaseId.Id);

        public void Remove(string id) =>
        _purchases.DeleteOne(purchase => purchase.Id == id);
    }
}
