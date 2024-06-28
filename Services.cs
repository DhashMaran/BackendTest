using BackendTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace BackendTest.Services
{
    public class OrderService
    {
        public List<Package> SplitOrderIntoPackages(List<Item> items)
        {
            List<Package> packages = new List<Package>();

            var totalOrderPrice = items.Sum(i => i.Price);

            // If the total price is less than or equal to $250, then 1 package
            if (totalOrderPrice <= 250)
            {
                packages.Add(new Package
                {
                    Items = items,
                    TotalWeight = items.Sum(i => i.Weight),
                    TotalPrice = totalOrderPrice,
                    CourierPrice = CalculateCourierPrice(items.Sum(i => i.Weight))
                });
                return packages;
            }

            // Sort items by weight in descending order for better weight distribution
            items = items.OrderByDescending(i => i.Weight).ToList();

            // Create the first order pakcge
            Package currentPackage = new Package();
            foreach (var item in items)
            {
                // if more then the price limit, create a new package
                if (currentPackage.TotalPrice + item.Price >= 250)
                {
                    packages.Add(currentPackage);
                    currentPackage = new Package();
                }

                currentPackage.Items.Add(item);
                currentPackage.TotalPrice += item.Price;
                currentPackage.TotalWeight += item.Weight;
            }

            
            if (currentPackage.Items.Any())
            {
                packages.Add(currentPackage);
            }

            // weight is redisributed 
            packages = RedistributeWeights(packages);

            // Calculate courier price for each package
            foreach (var package in packages)
            {
                package.CourierPrice = CalculateCourierPrice(package.TotalWeight);
            }

            return packages;
        }

        // the weight price
        private decimal CalculateCourierPrice(int weight)
        {
            if (weight <= 200) return 5;
            if (weight <= 500) return 10;
            if (weight <= 1000) return 15;
            return 20;
        }

        // redistribute weights equally
        private List<Package> RedistributeWeights(List<Package> packages)
        {
            var allItems = packages.SelectMany(p => p.Items).ToList();
            var totalWeight = allItems.Sum(i => i.Weight);
            var averageWeight = totalWeight / packages.Count;

            
            packages.ForEach(p => p.Items.Clear());
            packages.ForEach(p => { p.TotalWeight = 0; p.TotalPrice = 0; });

            
            foreach (var item in allItems.OrderByDescending(i => i.Weight))
            {
                var lightestPackage = packages.OrderBy(p => p.TotalWeight).FirstOrDefault(p => p.TotalPrice + item.Price < 250);
                if (lightestPackage != null)
                {
                    lightestPackage.Items.Add(item);
                    lightestPackage.TotalWeight += item.Weight;
                    lightestPackage.TotalPrice += item.Price;
                }
            }

            return packages;
        }
    }
}
