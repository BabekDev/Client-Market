using Client_Market.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Client_Market.ViewModels
{
    class AuthViewModel
    {
        public static event PropertyChangedEventHandler PropertyChanged;
        public static DispatcherTimer timer;
        public static string codePassword;

        private static bool checkAuth;
        private static string login;
        private static string code;

        private const string TEXT_EMAIL = "Check your email";
        private const string TEXT_INCORRECT = "Incorrect data, check again";
        private const string TEXT_CODE_INCORRECT = "Incorrect password code";
        private const string IP = "127.0.0.1";
        private const int PORT = 7000;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        public ICommand commandAuth => new RelayCommand(() =>
        {
            if (CheckEmail())
            {
                ConnectServer(ip:IP, port:PORT, value:Login);
                LoggerDebug.getInstance($"Sending a request to the server {IP}:{PORT}/{Login}");
            }
        });

        public ICommand commandProcced => new RelayCommand(() =>
        {
            if (CheckCode())
            {
                ConnectServer(ip: IP, port: PORT, value: "Good, auth user");
                LoggerDebug.getInstance($"Sending a request to the server {IP}:{PORT}/{Login}");
                checkAuth = true;
            }
        });

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

                codePassword = answer.ToString();
                tcpSocket.Shutdown(SocketShutdown.Both);
                tcpSocket.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool CheckEmail()
        {
            if (login != null)
            {
                LoggerDebug.getInstance($"Mail check successful");
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex rex = new Regex(strRegex);

                return (rex.IsMatch(login));
            }
            else
            {
                return false;
            }
        }

        public static bool CheckCode() => codePassword.Equals(code);
        public static string Text_Email() => TEXT_EMAIL;
        public static bool Check_Auth() => checkAuth;
        public static string Text_Incorrect() => TEXT_INCORRECT;
        public static string Text_Code_Incorrect() => TEXT_CODE_INCORRECT;
        public void OnPropertyChanged([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
