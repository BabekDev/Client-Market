using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_Market
{
    public class SellList
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Sell { get; set; }

        public SellList() { }
        public SellList(string name, string price, string sell)
        {
            Name = name;
            Price = price;
            Sell = sell;
        }

        public override string ToString()
        {
            return $"Name: {Name} / Price: {Price} / Sell: {Sell}";
        }
    }
}
