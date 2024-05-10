using AppTimeControl.AppDataClasses;
using AppTimeControl.Encrypting;
using AppTimeControl.MessageBoxPressets;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace AppTimeControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string username;
        private AppData appData;
        private Thread mainTimerThread;
        private DispatcherTimer mainUITimer;
        private SecurityData securityData;

        public MainWindow()
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")) ||
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json")) ||
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "security.data")))
            {
                foreach(string pathToFile in new string[] { Path.Combine(Directory.GetCurrentDirectory(), "app_data.json"), Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"), Path.Combine(Directory.GetCurrentDirectory(), "security.data") })
                {
                    if (File.Exists(pathToFile))
                    {
                        File.Delete(pathToFile);
                    }
                }
                mainTimerThread = null;
                mainUITimer = null;
                WelcomeWindow welcomeWindow = new WelcomeWindow();
                welcomeWindow.ShowDialog();
            }
            InitializeComponent();
            username = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"))).UserNickname;
            appData = JsonConvert.DeserializeObject<AppData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")));
            securityData = JsonConvert.DeserializeObject<SecurityData>(Encrypter.DecryptString(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "security.data"))));
            AuthenticationWindow.password = securityData.SecretPassword;
            if (appData.LastTimeOpened.ToString("D") != DateTime.Now.ToString("D"))
            {
                foreach (ApplicationInformation app in appData.Apps)
                {
                    app.TimeDone = TimeSpan.Zero;
                }
            }
            appData.LastTimeOpened = DateTime.Now;
            UITextChanger.ChangeGreetingText(ref GreetingTB, ref username);
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
            mainTimerThread = new Thread(TimerLoop);
            mainUITimer = new DispatcherTimer();
            mainUITimer.Interval = TimeSpan.FromMilliseconds(1000);
            mainUITimer.Tick += UILoop;
            this.Activated += (object sender, EventArgs e) => { mainUITimer.Start(); };
            this.Deactivated += (object sender, EventArgs e) => { mainUITimer.Stop(); };
            Notificator.SendNotification("Hello, World!");
            mainTimerThread.Start();
            mainUITimer.Start();
            GC.Collect();
        }

        private void UILoop(object sender, EventArgs e)
        {
            username = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"))).UserNickname;
            UITextChanger.ChangeGreetingText(ref GreetingTB, ref username);
            if (AppsLB.SelectedItem != null && AppsLB.Items.Count != 0)
            {
                PropertiesGrid.Visibility = Visibility.Visible;
                UITextChanger.ShowStats(ref AppNameTB,
                    ref ProcessNameTB,
                    ref TimeLeftTB,
                    ref TimeLeftPB,
                    ref TotalTimeTB,
                    appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()));
            }
        }

        private void TimerLoop()
        {
            try
            {
                while (mainTimerThread.IsAlive)
                {
                    if (appData.Apps.Count > 0)
                    {
                        foreach (ApplicationInformation app in appData.Apps)
                        {
                            if (!app.IsPaused)
                            {
                                Process[] appProcess = Process.GetProcessesByName(app.ProccessName);
                                if (appProcess.Length > 0)
                                {
                                    app.TimeDone = TimeSpan.FromSeconds(app.TimeDone.TotalSeconds + 1);
                                    app.WorkedInTotal = TimeSpan.FromSeconds(app.WorkedInTotal.TotalSeconds + 1);
                                    if (app.TimeLimit > TimeSpan.FromMinutes(5) && (app.TimeLimit - app.TimeDone).TotalSeconds == 300)
                                    {
                                        Notificator.SendNotification($"5 minutes left for {app.AppName} ({app.ProccessName})");
                                    }
                                    else if(app.TimeLimit >= TimeSpan.FromMinutes(1) && (app.TimeLimit - app.TimeDone).TotalSeconds == 60)
                                    {
                                        Notificator.SendNotification($"1 minute left for {app.AppName} ({app.ProccessName})");
                                    }
                                    else if((app.TimeLimit - app.TimeDone).TotalSeconds == 30)
                                    {
                                        Notificator.SendNotification($"30 seconds left for {app.AppName} ({app.ProccessName})");
                                    }
                                    if (app.TimeDone >= app.TimeLimit)
                                    {
                                        foreach (Process process in appProcess)
                                        {
                                            process.Kill();
                                        }
                                    }
                                }
                                appProcess = null;
                            }
                        }
                        if (appData.LastTimeOpened.ToString("D") != DateTime.Now.ToString("D"))
                        {
                            foreach (ApplicationInformation app in appData.Apps)
                            {
                                app.TimeDone = TimeSpan.Zero;
                            }
                            appData.LastTimeOpened = DateTime.Now;
                        }
                    }
                    AppData.SaveToFile(ref appData);
                    Thread.Sleep(1000);
                }
            }
            catch { }
        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            bool canCreate = true;
            if (securityData.CreatingNewListener)
            {
                canCreate = MessBox.AskPassword();
            }
            if (canCreate)
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
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppsLB.SelectedItem == null || AppsLB.Items.Count == 0)
            {
                return;
            }
            bool canRemove = true;
            if (securityData.RemovingListener)
            {
                canRemove = MessBox.AskPassword();
            }
            if (canRemove)
            {
                appData.Apps.Remove(appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()));
                UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
                SearchTB.Text = String.Empty;
            }
        }

        private void AppsLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AppsLB.SelectedItem == null || AppsLB.Items.Count == 0)
            {
                PropertiesGrid.Visibility = Visibility.Hidden;
                return;
            }
            PropertiesGrid.Visibility = Visibility.Visible;
            UITextChanger.ShowStats(ref AppNameTB,
                ref ProcessNameTB,
                ref TimeLeftTB,
                ref TimeLeftPB,
                ref TotalTimeTB,
                appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()));
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

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            CreationWindow creation = new CreationWindow(appData.Apps.Select(x => x.AppName).ToArray<string>(), appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()), ref securityData);
            creation.ShowDialog();
            if (creation.listener == null)
            {
                return;
            }
            int index = appData.Apps.IndexOf(appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()));
            appData.Apps[index].AppName = creation.listener.AppName;
            appData.Apps[index].ProccessName = creation.listener.ProccessName;
            appData.Apps[index].TimeLimit = creation.listener.TimeLimit;
            SearchTB.Text = String.Empty;
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
        }

        private void PrintInfoAboutSelectedMIBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppsLB.SelectedItem == null || AppsLB.Items.Count == 0)
            {
                PropertiesGrid.Visibility = Visibility.Hidden;
                return;
            }
            var app = appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString());
            string message = $"Application name: {app.AppName}\n" +
                            $"Process name: {app.ProccessName}\n" +
                            $"Time left: {app.TimeLimit - app.TimeDone}\n" +
                            $"Total time: {app.WorkedInTotal}\n";
            if (app.IsPaused)
            {
                message += "----------\nPAUSED";
            }
            Notificator.SendNotification(message);
        }

        private void ShowWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            bool canShow = true;
            if (securityData.ShowingWindow)
            {
                canShow = MessBox.AskPassword();
            }
            if (canShow)
            {
                this.Show();
                mainUITimer.Start();
            }
        }

        private void FinallyCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            bool canClose = true;
            if (securityData.ClosingWindow)
            {
                canClose = MessBox.AskPassword();
            }
            if (canClose)
            {
                mainTimerThread.Abort();
                mainUITimer.Stop();
                Environment.Exit(0);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            mainUITimer.Stop();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            bool canWork = MessBox.AskPassword();
            if (canWork)
            {
                SecuritySettings settings = new SecuritySettings(ref securityData);
                settings.ShowDialog();
                securityData = settings.SecurityData;
                AuthenticationWindow.password = securityData.SecretPassword;
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "security.data"), Encrypter.EncryptString(JsonConvert.SerializeObject(securityData)));
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppsLB.SelectedItem == null || AppsLB.Items.Count == 0)
            {
                PropertiesGrid.Visibility = Visibility.Hidden;
                return;
            }
            bool canPause = true;
            if (securityData.ChangingPauseStateOfListener)
            {
                canPause = MessBox.AskPassword();
            }
            if (canPause)
            {
                appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()).Pause();
            }
        }
    }
}
