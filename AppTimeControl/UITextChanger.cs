using AppTimeControl.AppDataClasses;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

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

    }
}
