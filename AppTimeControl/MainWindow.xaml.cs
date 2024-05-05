using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using AppTimeControl.AppDataClasses;
using Newtonsoft.Json;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string username;
        private AppData appData;

        public MainWindow()
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")) ||
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json")))
            {
                WelcomeWindow welcomeWindow = new WelcomeWindow();
                welcomeWindow.Show();
                this.Close();
            }
            InitializeComponent();
            username = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"))).UserNickname;
            appData = JsonConvert.DeserializeObject<AppData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")));
            UITextChanger.ChangeGreetingText(ref GreetingTB, ref username);
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            CreationWindow creation = new CreationWindow(appData.Apps.Select(x => x.AppName).ToArray<string>());
            creation.ShowDialog();
            if (creation.listener == null)
            {
                return;
            }
            appData.Apps.Add(creation.listener);
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(AppsLB.SelectedItem == null || AppsLB.Items.Count == 0)
            {
                return;
            }
            appData.Apps.Remove(appData.Apps.Where(x => x.AppName == AppsLB.SelectedItem.ToString()).First());
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
            SearchTB.Text = String.Empty;
        }

        private void AppsLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void SearchTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            AppsLB.SelectedItem = null;
            if (string.IsNullOrEmpty(SearchTB.Text.Trim()))
            {
                UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
                return;
            }
            var sortedList = appData.Apps.Where(x => x.AppName.ToLower().Contains(SearchTB.Text.Trim().ToLower())).ToList();
            UITextChanger.AddItemsToList(ref AppsLB, ref sortedList);
            sortedList.Clear();
        }
    }
}
