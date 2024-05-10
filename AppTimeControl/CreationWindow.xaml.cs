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
        private enum Mode
        {
            Creating, Editing
        }

        public ApplicationInformation listener = null;
        private string[] bannedNames;
        private Mode mode;

        private string beginName;
        private string beginProcessName;
        private TimeSpan beginTimeLimit;

        private bool changeAppName;
        private bool changeProcessName;
        private bool changeTimeLimit;

        public CreationWindow(string[] _bannedNames)
        {
            InitializeComponent();
            bannedNames = _bannedNames;
            foreach (string time in new string[] {
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
            TopTextTB.Text = "Let's create a new listener!";
            this.Title = "AppTimeControl - Creating new listener";
            mode = Mode.Creating;
        }

        public CreationWindow(string[] _bannedNames, ApplicationInformation app, ref SecurityData security)
        {
            InitializeComponent();
            bannedNames = _bannedNames;
            foreach (string time in new string[] {
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
            LimitCB.Text = app.TimeLimit.ToString();
            ProcessNameTB.Text = app.ProccessName;
            AppNameTB.Text = app.AppName;
            TopTextTB.Text = "Let's edit it!";
            this.Title = "AppTimeControl - Editing listener";
            beginName = app.AppName;
            beginProcessName = app.ProccessName;
            beginTimeLimit = app.TimeLimit;
            changeAppName = security.ChangingAppNameOfListener;
            changeProcessName = security.ChangingProcessNameOfListener;
            changeTimeLimit = security.ChangingTimLimitOfListener;
            CreateBtn.Content = "Apply";
            mode = Mode.Editing;
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
                    TimeSpan.Parse(LimitCB.Text.Trim()) <= TimeSpan.Zero ||
                    TimeSpan.Parse(LimitCB.Text.Trim()) >= new TimeSpan(24, 0, 0))
                {
                    throw new Exception("None of the fields can be null or empty!\nAnd time limit can't be more than 24 hours!");
                }
                if (bannedNames.Contains<string>(AppNameTB.Text.Trim()))
                {
                    if (mode == Mode.Creating)
                    {
                        throw new Exception("You are not allowed to create two listeners with the same name!");
                    }
                    else
                    {
                        if (!AppNameTB.Text.Trim().Equals(beginName))
                        {
                            throw new Exception("You are not allowed to give a used name!");
                        }
                    }
                }
                bool canCreate = true;
                if (mode == Mode.Editing)
                {
                    canCreate = false;
                    bool alreadyAskedPassword = false;
                    if (changeAppName && !beginName.Equals(AppNameTB.Text.Trim()) && !alreadyAskedPassword)
                    {
                        canCreate = MessBox.AskPassword();
                        alreadyAskedPassword = true;
                    }
                    if (changeProcessName && !beginProcessName.Equals(ProcessNameTB.Text.Trim()) && !alreadyAskedPassword)
                    {
                        canCreate = MessBox.AskPassword();
                        alreadyAskedPassword = true;
                    }
                    if (changeTimeLimit && !beginTimeLimit.Equals(TimeSpan.Parse(LimitCB.Text.Trim())) && !alreadyAskedPassword)
                    {
                        canCreate = MessBox.AskPassword();
                        alreadyAskedPassword = true;
                    }
                }
                if (canCreate)
                {
                    if (ProcessNameTB.Text.ToLower().Trim().Equals("apptimecontrol"))
                    {
                        throw new Exception("You can't set AppTimeControl as a listener in AppTimeControl!");
                    }
                    listener = new ApplicationInformation(ProcessNameTB.Text.Trim(), AppNameTB.Text.Trim(), TimeSpan.Parse(LimitCB.Text.Trim()));
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessBox.ShowError(ex.Message);
            }
        }
    }
}
