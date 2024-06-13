using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server_Market
{
    public class ServerObject
    {
        private const string EMAIL = "babekbaratov2021@gmail.com";
        private const string PASSWORD = "Babek123123";
        private const string SUBJECT = "Coffee Market";
        private const string IP = "127.0.0.1";
        private const int PORT = 7000;
        private static string recipemail { get; set; }
        private static string text { get; set; }
        private static string login { get; set; }
        private static ClientObject clientObject = new ClientObject();
        private static string Client { get; set; }


        public ServerObject()
        {
            Server();
        }

        private static void Server()
        {
            try
            {
                var tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
                var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpSocket.Bind(tcpEndPoint);
                tcpSocket.Listen(127);
                Console.WriteLine("Server Start");

                while (true)
                {
                    var listener = tcpSocket.Accept();
                    var buffer = new byte[256];
                    var size = 0;
                    var data = new StringBuilder();

                    do
                    {
                        size = listener.Receive(buffer);
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    } while (listener.Available > 0);

                    string hash = GetHash(data.ToString());
                    if (hash.Equals("0GaQrkculKpG+xLeRTbW0Q==")) clientObject.AuthResult(true);

                    string message = CheckContent(data.ToString());
                    if (CheckEmail(data.ToString()))
                    {
                        listener.Send(Encoding.UTF8.GetBytes($"{message}"));
                        login = data.ToString();
                        Console.WriteLine($"Email: {data} | Code: {message}");
                    }
                    if (data.ToString().Equals("JSON"))
                    {
                        string json = clientObject.LoadProductData();
                        listener.Send(Encoding.UTF8.GetBytes($"{json}"));
                        Console.WriteLine($"Send API {data.ToString()}");
                        listener.Send(Encoding.UTF8.GetBytes(clientObject.Market_API(data.ToString())));
                    }

                    string str = data.ToString();
                    string[] result = str.Split('/');

                    if (str.Equals($"LoadData"))
                    {
                        string json = clientObject.LoadProductData();
                        listener.Send(Encoding.UTF8.GetBytes($"{json}"));
                        json = null;
                    }
                    else if (str.Equals($"LoadData_ADMIN"))
                    {
                        string json = clientObject.LoadSellData();
                        listener.Send(Encoding.UTF8.GetBytes($"{json}"));
                        json = null;
                    }
                    else if (result[0].Equals($"AddProduct"))
                    {
                        clientObject.AddProductData(result[1], result[2]);
                        Console.WriteLine("Add Product in DataBase");
                    }
                    else if (result[0].Equals($"Sell"))
                    {
                        Console.WriteLine($"User [{login}] buy [{result[1]}] / [{result[2]}$]");
                        clientObject.AddSellData(result[1], result[2], login);
                        Console.WriteLine("Save Data");
                    }
                    else if (result[0].Equals($"SellTG"))
                    {
                        Console.WriteLine($"User [{result[1]}] buy [{result[2]}] / [{result[3]}$]");
                        clientObject.AddSellData(result[2], result[3], result[1]);
                        Console.WriteLine("Save Data");
                    }

                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string CheckContent(string content)
        {
            if (CheckEmail(content))
            {
                Client = content;
                text = RandomNum().ToString();
                recipemail = content;
                SendMessage("smtp.gmail.com", EMAIL, PASSWORD, recipemail, SUBJECT, text);
                if (!clientObject.CheckClient(content)) clientObject.AddClient(content);
                return text;
            }
            else
            {
                return String.Empty;
            }
        }

        private static bool CheckEmail(string content)
        {
            if (content != null)
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex rex = new Regex(strRegex);

                return (rex.IsMatch(content));
            }
            return false;
        }

        private static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        private static int RandomNum()
        {
            Random rnd = new Random();
            int value = rnd.Next(100000, 999999);
            return value;
        }

        private static bool SendMessage(string smtpServer, string from, string password,
        string mailto, string caption, string message, string attachFile = null, int port = 587)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = $"Your passcode, do not share it with anyone: {message}";
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = port;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                client.Dispose();
                mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
