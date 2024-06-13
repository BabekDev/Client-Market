using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Server_Market
{
    class ClientObject
    {
        private List<string> clients = new List<string>();
        private List<string> sellers = new List<string>();
        private Model_Coffee_Market model = new Model_Coffee_Market();
        private ObservableCollection<Coffee> coffees = new ObservableCollection<Coffee>();
        private ObservableCollection<SellList> client_list = new ObservableCollection<SellList>();
        private Coffee coffee = new Coffee();

        public void AddClient(string email)
        {
            clients.Add(email);
        }

        public bool CheckClient(string email)
        {
            foreach (var login in model.UsersList)
            {
                if (email.Equals(login.Login))
                {
                    return true;
                }
            }
            return false;
        }

        public void AuthResult(bool check) => SaveData();

        public string LoadSellData()
        {
            foreach (var item in model.SellList)
            {
                client_list.Add(new SellList(item.Name, item.Price, item.Client));
            }

            string json = JsonConvert.SerializeObject(client_list);
            return json;
        }

        private void SaveData()
        {
            if(clients != null)
            {
                foreach (var client in clients)
                {
                    var modelClient = new Users { Login = client };
                    model.UsersList.Add(modelClient);
                    model.SaveChanges();
                }
            }
        }

        public void AddProductData(string name, string price)
        {
            var modelClient = new Product { Name = name, Price= price };
            model.ProductList.Add(modelClient);
            model.SaveChanges();
        }

        public string LoadProductData()
        {
            foreach (var item in model.ProductList)
            {
                coffees.Add(new Coffee(item.Name, item.Price));
            }

            string json = JsonConvert.SerializeObject(coffees);
            return json;
        }

        public string LoadData_API(string value)
        {
            foreach (var item in model.ProductList)
            {
                coffees.Add(new Coffee(item.Name, item.Price));
            }

            if (value.Equals("JSON"))
            {
                return JsonConvert.SerializeObject(coffees);
            }
            else if (value.Equals("XML"))
            {
                //XNode node = JsonConvert.DeserializeXNode(LoadProductData(), "Root");
                //return node.ToString();
                return "Not available";
            }
            else
            {
                return null;
            }
        }

        public void AddSellData(string name, string price, string client)
        {
            var modelClient = new SellProduct { Name = name, Price = price, Client = client };
            model.SellList.Add(modelClient);
            model.SaveChanges();
        }

        public string Market_API(string value)
        {
            if (value.Equals("XML"))
            {
                return LoadData_API(value);
            }
            else if (value.Equals("JSON"))
            {
                return LoadData_API(value);
            }
            else
            {
                return "Error, invalid value";
            }
        }
    }
}
