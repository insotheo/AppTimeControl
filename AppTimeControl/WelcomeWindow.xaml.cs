using System.Windows;
using System;
using AppTimeControl.AppDataClasses;
using System.IO;
using Newtonsoft.Json;
using AppTimeControl.MessageBoxPressets;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private readonly string[] adjectives = { "Cool", "Secret", "Foggy", "Unusual", "Fake", "Beautiful", "Smart", "Impressive", "Happy", "Mystic", "Brave", "Sunny", "Gentle", "Dynamic", "Sneaky", "Lucky", "Magical", "Electric", "Daring", "Epic" };
        private readonly string[] nouns = { "Fox", "Frog", "Bear", "Gamer", "Spirit", "Witcher", "Assassin", "Master", "User", "Wolf", "Wizard", "Knight", "Ninja", "Captain", "Phoenix", "Dragon", "Pirate", "Tiger", "Star", "Joker" };
        private string randomNickname;
        private Random rnd;

        public WelcomeWindow()
        {
            rnd = new Random();
            generateNewRandomNickname();
            InitializeComponent();
            NicknameTB.Text = randomNickname;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void generateNewRandomNickname()
        {
            randomNickname = adjectives[rnd.Next(0, adjectives.Length)] + "_" + nouns[rnd.Next(0, rnd.Next(0, nouns.Length))] + rnd.Next(0, 999).ToString();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isSure = true;
            if (string.IsNullOrEmpty(NicknameTB.Text) || NicknameTB.Text.Trim() == randomNickname)
            {
                isSure = false;
                if(MessageBox.Show($"Are you sure you want to use a random nickname (\"{randomNickname}\")?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    isSure = true;
                }
            }
            if(isSure)
            {
                    UserData newUser = new UserData(NicknameTB.Text.Trim());
                    FileStream userFile = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"));
                    userFile.Close();
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"), JsonConvert.SerializeObject(newUser));
                    AppData newAppData = new AppData();
                    FileStream appDataFile = File.Create(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json"));
                    appDataFile.Close();
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json"), JsonConvert.SerializeObject(newAppData, Formatting.Indented));
            }
        }

        private void MakeNewRandomNicknameBtn_Click(object sender, RoutedEventArgs e)
        {
            generateNewRandomNickname();
            NicknameTB.Text = randomNickname;
        }
    }
}
