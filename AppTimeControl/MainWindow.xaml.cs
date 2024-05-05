using System.IO;
using System.Windows;
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
            CreationWindow creation = new CreationWindow();
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
            appData.Apps.RemoveAt(AppsLB.SelectedIndex);
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
        }
    }
}
