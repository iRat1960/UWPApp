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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPApp
{
    public sealed partial class UsersPage : Page
    {
        User user;

        public UsersPage()
        {
            this.InitializeComponent();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            using (HFContext db = new HFContext())
            {
                if (user != null)
                {
                    user.Name = nameBox.Text;
                    db.Users.Update(user);
                }
                else
                {
                    db.Users.Add(new User { Name = nameBox.Text });
                }
                db.SaveChanges();
            }
            GoToMainPage();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                int id = (int)e.Parameter;
                using (HFContext db = new HFContext())
                {
                    user = db.Users.FirstOrDefault(c => c.Id == id);
                }
            }
            if (user != null)
            {
                headerBlock.Text = "Редактирование аккаунта";
                nameBox.Text = user.Name;
            }
        }

        private void GoToMainPage()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            else
                Frame.Navigate(typeof(MainPage));
        }
    }
}
