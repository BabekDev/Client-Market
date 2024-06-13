using Client_Market.ViewModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client_Market.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthView.xaml
    /// </summary>
    /// 
    public partial class AuthView : UserControl
    {

        public AuthView()
        {
            LoggerDebug.getInstance($"Start view AuthView");
            InitializeComponent();
            DataContext = new AuthViewModel();
        }

        private void SnackbarMessage_ActionClick(object sender, RoutedEventArgs e)
        {
            snackbarTwo.IsActive = false;
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            LoggerDebug.getInstance($"Button click {authButton}");

            if (AuthViewModel.CheckEmail())
            {
                LoggerDebug.getInstance($"Correct email");
                snackbarTwo.IsActive = false;
                msgSnackbar.Content = AuthViewModel.Text_Email();
                snackbarTwo.IsActive = true;

                passcodeTextBox.Visibility = Visibility.Visible;
                passcodeLabel.Visibility = Visibility.Visible;
                proceedButton.Visibility = Visibility.Visible;
                authButton.IsEnabled = false;

                AuthViewModel.timer = new DispatcherTimer();
                AuthViewModel.timer.Interval = TimeSpan.FromSeconds(1);
                AuthViewModel.timer.Start();
                LoggerDebug.getInstance($"Start Timer");
                AuthViewModel.timer.Tick += Timer_Tick;
            }
            else
            {
                LoggerDebug.getInstance($"Invalid email");
                snackbarTwo.IsActive = false;
                msgSnackbar.Content = AuthViewModel.Text_Incorrect();
                snackbarTwo.IsActive = true;
            }
        }

        private byte dtTimer = 60;
        public void Timer_Tick(object sender, EventArgs e)
        {
            dtTimer--;
            timeExpLabel.Content = $"Code expiration date: {dtTimer.ToString()}";
            timeExpLabel.Visibility = Visibility.Visible;

            if (dtTimer == 0)
            {
                dtTimer = 120;
                AuthViewModel.codePassword = "!&!&!&!";
                AuthViewModel.timer.Stop();
                timeExpLabel.Content = "Code expired!";
                authButton.IsEnabled = true;
            }
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            LoggerDebug.getInstance($"Button click {proceedButton}");
            if (!AuthViewModel.CheckCode())
            {
                LoggerDebug.getInstance($"CheckCode successfully");
                snackbarTwo.IsActive = false;
                msgSnackbar.Content = AuthViewModel.Text_Code_Incorrect();
                snackbarTwo.IsActive = true;
            }
            else
            {
                LoggerDebug.getInstance($"Invalid value (CheckCode)");
                marketView.Visibility = Visibility.Visible;
            }
        }
    }
}
