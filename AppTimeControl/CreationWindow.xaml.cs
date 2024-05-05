using AppTimeControl.AppDataClasses;
using AppTimeControl.MessageBoxPressets;
using System;
using System.Linq;
using System.Windows;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for CreationWindow.xaml
    /// </summary>
    public partial class CreationWindow : Window
    {
        public ApplicationInformation listener = null;

        private string[] bannedNames;

        public CreationWindow(string[] _bannedNames)
        {
            InitializeComponent();
            bannedNames = _bannedNames;
            foreach(string time in new string[] { 
                TimeSpan.FromHours(0.5f).ToString(),
                TimeSpan.FromHours(1f).ToString(),
                TimeSpan.FromHours(2f).ToString(),
                TimeSpan.FromHours(2.5f).ToString(), 
                TimeSpan.FromHours(3f).ToString(), 
                TimeSpan.FromHours(3.5f).ToString(), 
                TimeSpan.FromHours(4f).ToString(), 
                TimeSpan.FromHours(5f).ToString(), 
                TimeSpan.FromHours(10f).ToString(), 
                TimeSpan.FromHours(12f).ToString(), 
            })
            {
                LimitCB.Items.Add(time);
            }
            LimitCB.SelectedIndex = 2;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            listener = null;
            this.Close();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ProcessNameTB.Text.Trim()) ||
                    string.IsNullOrEmpty(AppNameTB.Text.Trim()) ||
                    string.IsNullOrEmpty(LimitCB.Text.Trim()) ||
                    TimeSpan.Parse(LimitCB.Text.Trim()) == TimeSpan.Zero)
                {
                    throw new Exception("None of the fields can be null or empty!");
                }
                if (bannedNames.Contains<string>(AppNameTB.Text.Trim()))
                {
                    throw new Exception("You are not allowed to create two listeners with the same name!");
                }
                listener = new ApplicationInformation(ProcessNameTB.Text.Trim(), AppNameTB.Text.Trim(), TimeSpan.Parse(LimitCB.Text.Trim()));
                this.Close();
            }
            catch(Exception ex)
            {
                MessBox.ShowError(ex.Message);
            }
        }
    }
}
