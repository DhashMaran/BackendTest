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

            // If the total price is less than or equal to 250, all items can be in one package
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

            // Sort items by price in ascending order
            items = items.OrderBy(i => i.Price).ToList();

            // Create  package
            Package currentPackage = new Package();
            foreach (var item in items)
            {
                
                if (currentPackage.TotalPrice + item.Price > 250)
                {
                    
                    packages.Add(currentPackage);
                    // Create another new package
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

        // Method to calculate the courier price based on weight
        private decimal CalculateCourierPrice(int weight)
        {
            if (weight <= 200) return 5;    
            if (weight <= 500) return 10;   
            if (weight <= 1000) return 15;  
            return 20;                      
        }
    }
}
