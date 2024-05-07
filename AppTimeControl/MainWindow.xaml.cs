using AppTimeControl.AppDataClasses;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

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

        public MainWindow()
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")) ||
                !File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json")))
            {
                WelcomeWindow welcomeWindow = new WelcomeWindow();
                welcomeWindow.ShowDialog();
                this.Close();
            }
            InitializeComponent();
            username = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "user_data.json"))).UserNickname;
            appData = JsonConvert.DeserializeObject<AppData>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "app_data.json")));
            UITextChanger.ChangeGreetingText(ref GreetingTB, ref username);
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
            mainTimerThread = new Thread(TimerLoop);
            Notificator.SendNotification("Hello, World!");
            mainTimerThread.Start();
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
                            Process[] appProcess = Process.GetProcessesByName(app.ProccessName);
                            if (appProcess.Length > 0)
                            {
                                app.TimeDone = TimeSpan.FromSeconds(app.TimeDone.TotalSeconds + 1);
                                app.WorkedInTotal = TimeSpan.FromSeconds(app.WorkedInTotal.TotalSeconds + 1);
                                if (app.TimeLimit.TotalSeconds - app.TimeLimit.TotalSeconds == 300)
                                {
                                    Notificator.SendNotification("5 minutes left for " + app.AppName + $"({app.ProccessName})");
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
                        AppData.SaveToFile(ref appData);
                        Thread.Sleep(1000);
                    }
                }
            }
            catch { }
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
            if (AppsLB.SelectedItem == null || AppsLB.Items.Count == 0)
            {
                return;
            }
            appData.Apps.Remove(appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()));
            UITextChanger.AddItemsToList(ref AppsLB, ref appData.Apps);
            SearchTB.Text = String.Empty;
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
            CreationWindow creation = new CreationWindow(appData.Apps.Select(x => x.AppName).ToArray<string>(), appData.Apps.First(x => x.AppName == AppsLB.SelectedItem.ToString()));
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

        private void UpdateStatsBtn_Click(object sender, RoutedEventArgs e)
        {
            var selItem = AppsLB.SelectedItem;
            AppsLB.SelectedItem = null;
            AppsLB.SelectedItem = selItem;
            selItem = null;
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //Only DEV
            mainTimerThread.Abort();
        }

    }
}
