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
using UWPApp.Controls;

namespace UWPApp
{
    public sealed partial class FinancesPage : Page
    {
        PagesView view;

        public FinancesPage()
        {
            view = new PagesView { Caption = "Состояние счетов", SubCaption = "Текущая дата - " + DateTime.Now.ToLongDateString() };
            InitializeComponent();
            DataContext = view;
            menu.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                textAccount.Text = e.Parameter.ToString();
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menu.SelectedItems != null)
            {
                int ind = menu.SelectedIndex;
                var item = menu.SelectedItem;
                string text = (((item as ListBoxItem).Content as StackPanel).Children[1] as TextBlock).Text;
                view.Caption = text;
                int ct = grid.Children.Count - 1;
                for (int i = ct; i > 0; i--)
                {
                    //if (grid.Children[i].GetType().Equals(typeof(OptionsControl)) ||
                    //    grid.Children[i].GetType().Equals(typeof(CategoryControl)) ||
                    //    grid.Children[i].GetType().Equals(typeof(SubCategoryControl)))
                    //    grid.Children.RemoveAt(i);

                }
                dynamic cont = null;
                switch (ind)
                {
                    case 0:
                        cont = new WrapPanel.WrapPanel();
                        using (HFContext db = new HFContext())
                        {
                            IQueryable<BankAccountView> views = 
                                from c in db.BankAccounts
                                //join d in db.Entries on c.Id equals d.CardsId
                                select new BankAccountView
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Currency = c.Currency,
                                    AccountType = c.Cash == CashType.Наличная ? "Кошелёк" : (c.CardType == CardType.Нет ? "Расчетный счет" : "Банковская карта"),
                                    Locking = c.Locking,
                                    Payment = c.Payment,
                                    Total = c.Total
                                };
                            foreach (BankAccountView account in views)
                            {
                                CheckingAccount card = new CheckingAccount(account);
                                card.Margin = new Thickness(20);
                                (cont as WrapPanel.WrapPanel).Children.Add(card);
                            }
                        }
                        
                        //BankAccount account = new BankAccount
                        //{
                        //    Id = 0,
                        //    Name = "Карта ВТБ **** 4516",
                        //    CardType = CardType.Дебетовая,
                        //    Cash = CashType.Безналичная,
                        //    Currency = CurrencyType.RUB,
                        //    Total = 15000,
                        //    Payment = PaymentSystem.Мир,
                        //    DateTotal = DateTime.Now,
                        //    Locking = false
                        //};
                        break;
                    case 1:
                        //cont = new CategoryControl();
                        //(cont as CategoryControl).RegisterDelegate(listCategory_SelectionChanged);
                        //(cont as CategoryControl).RegisterDelegate(CategoryAdd);
                        //view.SubCaption = "Подкатегории";
                        //SubCategoryControl sub = new SubCategoryControl();
                        //grid.Children.Add(sub);
                        //Grid.SetColumn(sub, 1);
                        //Grid.SetRow(sub, 1);
                        break;
                    case 2:
                        //cont = new CategoryControl();
                        break;
                    case 3:
                        //cont = new CategoryControl();
                        break;
                    case 4:
                        //cont = new CategoryControl();
                        break;
                }
                if (cont != null)
                {
                    grid.Children.Add(cont);
                    Grid.SetColumn(cont, 0);
                    Grid.SetRow(cont, 1);
                }
            }
        }
    }

    public class BankAccountView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CurrencyType Currency { get; set; }
        public string AccountType { get; set; }
        public PaymentSystem Payment { get; set; }
        public decimal Total { get; set; }
        public bool Locking { get; set; }
    }
}
