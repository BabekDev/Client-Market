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
using System.Windows.Controls;
using System.Windows.Input;

namespace Client_Market.ViewModels
{
    class MarketViewModel
    {
        public static event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Coffee> output { get; set; } = new ObservableCollection<Coffee>();

        private const string IP = "127.0.0.1";
        private const int PORT = 7000;

        public static List<string> product = new List<string>();

        public MarketViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            ConnectServer(ip: IP, port: PORT, value: $"LoadData");
        }

        public void SellProduct(int index)
        {
            ConnectServer(ip: IP, port: PORT, value: $"Sell/{output[index].Name}/{output[index].Price}");
        }

        private void ConnectServer(string ip, int port, string value)
        {
            LoggerDebug.getInstance($"Sending a request to the server (ConnectServer)");
            try
            {
                var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
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
                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
