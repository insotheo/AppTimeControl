using System.ComponentModel;
using System.Windows;
using System;
using AppTimeControl.MessageBoxPressets;

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
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(PasswordTB.Password))
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
            catch(Exception ex)
            {
                MessBox.ShowError(ex.Message);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }
    }
}
