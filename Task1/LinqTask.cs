using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(x => x.Orders.Count() > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(x => (x, suppliers.Where(s => s.City == x.City)));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            var suppliersByCity = suppliers.GroupBy(x => x.City);
            return customers.Select(x => (x, suppliersByCity.Where(sc => sc.Key == x.City).SelectMany(sc => sc)));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(x => x.Orders.Any(x => x.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            return customers.Where(c => c.Orders.Length > 0).Select(x => (x, x.Orders.Min(c => c.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            return Linq4(customers).OrderBy(x => x.dateOfEntry).ThenBy(x => x.customer.Orders.Count()).ThenBy(x => x.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers.Where(x => !x.PostalCode.All(char.IsDigit) || string.IsNullOrEmpty(x.Region) || (!x.Phone.Contains('(')));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            /* example of Linq7result

             category - Beverages
	            UnitsInStock - 39
		            price - 18.0000
		            price - 19.0000
	            UnitsInStock - 17
		            price - 18.0000
		            price - 19.0000
             */

            return products.GroupBy(x => x.Category)
                .Select(productsByCategory => new Linq7CategoryGroup()
                {
                    Category = productsByCategory.Key,
                    UnitsInStockGroup = productsByCategory.GroupBy(p => p.UnitsInStock).Select(productsByUnitsInStock => new Linq7UnitsInStockGroup()
                    {
                        UnitsInStock = productsByUnitsInStock.Key,
                        Prices = productsByUnitsInStock.Select(p => p.UnitPrice),
                    }),
                });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            return products.OrderBy(p => p.UnitPrice).GroupBy(p => p.UnitPrice <= cheap ? cheap : p.UnitPrice <= middle ? middle : expensive).Select(m => (m.Key, m.Select(x => x)));
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            return customers.GroupBy(x => x.City).Select(customersByCity => (customersByCity.Key, (int)Math.Round(customersByCity.Average(c => c.Orders.Sum(o => o.Total))), (int)customersByCity.Average(c => c.Orders.Count())));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            return string.Concat(suppliers.Select(x => x.Country).Distinct().OrderBy(x => x.Length).ThenBy(x => x));
        }
    }
}