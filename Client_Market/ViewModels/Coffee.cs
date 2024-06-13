using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Market.ViewModels
{
    public class Coffee
    {
        public string Name { get; set; }
        public string Price { get; set; }

        public Coffee() { }
        public Coffee(string name, string price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"Name: {Name}   /   Price: {Price}";
        }
    }
}
