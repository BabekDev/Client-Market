using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Market_API
{
    public class LinkServer
    {
        private const string IP = "127.0.0.1";
        private const int PORT = 7000;

        public string ConnectServer(string value)
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

                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close();
                return answer.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
