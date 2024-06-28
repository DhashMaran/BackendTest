
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

            items = items.OrderBy(i => i.Price).ToList();

            Package currentPackage = new Package();
            foreach (var item in items)
            {
                if (currentPackage.TotalPrice + item.Price > 250)
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

            foreach (var package in packages)
            {
                package.CourierPrice = CalculateCourierPrice(package.TotalWeight);
            }

            return packages;
        }

        private decimal CalculateCourierPrice(int weight)
        {
            if (weight <= 200) return 5;
            if (weight <= 500) return 10;
            if (weight <= 1000) return 15;
            return 20;
        }
    }
}
