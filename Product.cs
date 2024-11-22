using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grape_Fruit_Bar
{
    internal class Product
    {
        public int Id { get; }
        public string Name { get; set; }
        public double Price { get; set; }
        public PriceType PriceType { get; set; }

        public Product(int id, string name, double price, PriceType priceType)
        {
            Id = id;
            Name = name;
            Price = price;
            PriceType = priceType;
        }

    }   
}
