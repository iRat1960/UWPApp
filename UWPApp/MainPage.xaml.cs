using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPApp
{
    public sealed partial class MainPage : Page
    {
        int defaultAccount = 0;
        HFContext db;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            db = new HFContext();
            //loadData(db);
            if (combo1.ItemsSource is null)
                combo1.ItemsSource = db.Users.ToList();
            if (db.Users.Count() > 0)
                combo1.SelectedIndex = defaultAccount;
            menu.SelectedIndex = 0;
        }

        private async void loadData(HFContext db)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("Files");
            var file = await folder.GetFileAsync("Categories.txt");
            var text = await FileIO.ReadTextAsync(file);

            List<Category> cont = Deserialize<List<Category>>(text);
            db.Categories.AddRange(cont);
            db.SaveChanges();
        }

        public static T Deserialize<T>(string json)
        {
            var bytes = Encoding.Unicode.GetBytes(json);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
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
            string prm = combo1.SelectedItem.ToString();
            switch (menuIndex)
            {
                case 1:
                    Frame.Navigate(typeof(FinancesPage), prm);
                    break;
                case 2:
                    Frame.Navigate(typeof(AnaliticsPage), prm);
                    break;
                case 3:
                    Frame.Navigate(typeof(SettingPage), prm);
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
            defaultAccount = combo1.SelectedIndex;
        }
    }
}
