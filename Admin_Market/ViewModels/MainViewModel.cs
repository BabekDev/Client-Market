using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Admin_Market.ViewModels
{
    class MainViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Coffee> output { get; set; } = new ObservableCollection<Coffee>();
        public ObservableCollection<SellList> output_sell { get; set; } = new ObservableCollection<SellList>();

        private const string IP = "127.0.0.1";
        private const int PORT = 7000;

        private string name;
        private string price;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public ICommand commandAddProduct => new RelayCommand(() => {
            if (Name.Length != 0 && Price.Length != 0)
            {
                ConnectServer($"AddProduct/{Name}/{Price}");
                MessageBox.Show($"Add Product: {Name}/{Price}$");
            }
        });


        public void ConnectServer(string value)
        {
            try
            {
                var tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
                var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var message = value;
                var data = Encoding.UTF8.GetBytes(message);
                tcpSocket.Connect(tcpEndPoint);
                tcpSocket.Send(data);

                var buffer = new byte[256];
                var size = 0;
                var answer = new StringBuilder();

                do
                {
                    size = tcpSocket.Receive(buffer);
                    answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                } while (tcpSocket.Available > 0);

                output = JsonConvert.DeserializeObject<ObservableCollection<Coffee>>(answer.ToString());
                output_sell = JsonConvert.DeserializeObject<ObservableCollection<SellList>>(answer.ToString());
                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
