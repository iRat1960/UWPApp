using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWPApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (HFContext db = new HFContext())
            {
                combo1.ItemsSource = db.Users.ToList();
                if (db.Users.Count() > 0)
                    combo1.SelectedIndex = 0; 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void calendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            //textBlock1.Text = "";

            //foreach (var d in calendarView.SelectedDates)
            //    textBlock1.Text += d.ToString("dd/MM/yyyy") + "\n";
        }

        private void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int menuIndex = menu.SelectedIndex;
            switch (menuIndex)
            {
                case 0:
                    break;
                case 1:
                    Frame.Navigate(typeof(FinancesPage), combo1.SelectedItem);
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UsersPage));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (combo1.SelectedItem != null)
            {
                User user = combo1.SelectedItem as User;
                if (user != null)
                    Frame.Navigate(typeof(UsersPage), user.Id);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (combo1.SelectedItem != null)
            {
                User user = combo1.SelectedItem as User;
                if (user != null)
                {
                    using (HFContext db = new HFContext())
                    {
                        db.Users.Remove(user);
                        db.SaveChanges();
                        combo1.ItemsSource = db.Users.ToList();
                    }
                }
            }
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
