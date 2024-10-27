/*using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PR_lab1;

public class PriceProcessor
{
    private const decimal EURToLEI = 4.5m; // Example conversion rate
    private const decimal USDToEUR = 0.85m; // Example conversion rate

    public ProcessedProducts Process(List<Product> products)
    {
        // Map: Convert currencies
        var convertedProducts = products.Select(product =>
        {
            decimal priceValue = 0;
            string currency = "";

            // Extract numeric value and currency symbol
            
            if (product.Price.Contains("$"))
            {
                priceValue = decimal.Parse(product.Price.Remove(product.Price[0]).Trim()) * USDToEUR;
                currency = "EUR";
            }
            else if (product.Price.Contains("lei"))
            {
                priceValue = decimal.Parse(product.Price.Replace("lei", "").Trim()) / EURToLEI;
                currency = "EUR";
            }

            product.PriceValue = priceValue;
            product.Currency = currency;

            return product;
        }).ToList();

        // Filter: Only include products between 20 and 100 EUR
        var filteredProducts = convertedProducts.Where(p => p.PriceValue >= 20 && p.PriceValue <= 100).ToList();

        // Reduce: Sum up the prices of filtered products
        decimal totalPrice = filteredProducts.Sum(p => p.PriceValue);

        // Create new processed data model
        return new ProcessedProducts
        {
            FilteredProducts = filteredProducts,
            TotalPrice = totalPrice,
            UTCTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }
}*/