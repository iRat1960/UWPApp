using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
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
                if (gridCont.Children.Count > 0)
                    gridCont.Children.Clear();
                dynamic cont = null;
                switch (ind)
                {
                    case 0:
                        cont = new AccountsControl();
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
                    gridCont.Children.Add(cont);
                    //Grid.SetColumn(cont, 0);
                    //Grid.SetRow(cont, 1);
                }
            }
        }
    }

    public class BankAccountView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CardType CardType { get; set; }
        public CurrencyType Currency { get; set; }
        public string AccountType { get; set; }
        public PaymentSystem Payment { get; set; }
        public decimal Total { get; set; }
        public bool Locking { get; set; }
    }
}
