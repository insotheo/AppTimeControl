using AppTimeControl.AppDataClasses;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppTimeControl
{
    internal static class UITextChanger
    {

        public static void ChangeGreetingText(ref TextBlock textBlock, ref string name)
        {
            string partOfTheDay;
            if (DateTime.Now.Hour > 4 && DateTime.Now.Hour < 10)
            {
                partOfTheDay = "morning";
            }
            else if (DateTime.Now.Hour >= 10 && DateTime.Now.Hour < 16)
            {
                partOfTheDay = "afternoon";
            }
            else if (DateTime.Now.Hour >= 16 && DateTime.Now.Hour < 22)
            {
                partOfTheDay = "evening";
            }
            else
            {
                partOfTheDay = "night";
            }
            textBlock.Text = $"Good {partOfTheDay}, {name}!";
        }

        public static void AddItemsToList(ref ListBox listBox, ref List<ApplicationInformation> apps)
        {
            listBox.Items.Clear();
            foreach (ApplicationInformation app in apps)
            {
                listBox.Items.Add(app.AppName);
            }
        }

        public static void ShowStats(ref TextBlock appNameTB, ref TextBlock processNameTB, ref TextBlock timeLeftTB, ref ProgressBar timeLeftPB, ref TextBlock totalTimeTB, ApplicationInformation app)
        {
            int percent = (int)(calcPercent(ref app.TimeLimit, ref app.TimeDone) * 100);
            appNameTB.Text = app.AppName;
            processNameTB.Text = "Process name: " + app.ProccessName;
            timeLeftTB.Text = $"{app.TimeDone.ToString()}/{app.TimeLimit.ToString()} ({percent}%)";
            timeLeftPB.Value = percent;
            if (timeLeftPB.Value >= 100)
            {
                timeLeftPB.Foreground = Brushes.Red;
            }
            totalTimeTB.Text = "Total time: " + app.WorkedInTotal.ToString();
        }

        private static float calcPercent(ref TimeSpan total, ref TimeSpan done)
        {
            return (float)Math.Round((total.TotalMilliseconds * done.TotalMilliseconds) / 100, 2);
        }

    }
}
