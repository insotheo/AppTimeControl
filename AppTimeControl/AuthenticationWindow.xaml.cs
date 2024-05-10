using AppTimeControl.MessageBoxPressets;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        internal static string password;
        public bool IsConfirmed = false;

        public AuthenticationWindow()
        {
            InitializeComponent();
            this.Closing += (object sender, CancelEventArgs e) => { PasswordTB.Password = String.Empty; };
            PasswordTB.Focus();
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfirmAndContinue();
        }

        private void ConfirmAndContinue()
        {
            try
            {
                if (string.IsNullOrEmpty(PasswordTB.Password))
                {
                    throw new Exception("You can't leave the password field blank!");
                }
                if (PasswordTB.Password.Equals(password))
                {
                    IsConfirmed = true;
                    this.Close();
                }
                else
                {
                    throw new Exception("The entered password is incorrect!");
                }
            }
            catch (Exception ex)
            {
                MessBox.ShowError(ex.Message);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }

        private void PasswordTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfirmAndContinue();
            }
            else if (e.Key == Key.Escape)
            {
                IsConfirmed = false;
                this.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IsConfirmed = false;
                this.Close();
            }
        }
    }
}
