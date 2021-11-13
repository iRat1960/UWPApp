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

namespace UWPApp.Controls
{
    public sealed partial class AccountsControl : UserControl
    {
        public AccountsControl()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (HFContext db = new HFContext())
            {
                DateTime date1 = DateTime.Now;
                //var query = db.Entries.Where(c => c.Category.Type == CategoriesType.Расходы).Sum(o => o.Amount);
                var query = db.Entries.Where(w => w.DateEntry <= date1).GroupBy(g => g.BankAccountId).
                    Select(s => new { s.Key, Total = s.Sum(o => o.Category.Type == CategoriesType.Доходы ? o.Amount : -o.Amount) });

                //var q = from c in db.Entries
                //        join d in db.Categories on c.CategoryId equals d.Id into gj
                //        from subd in gj.DefaultIfEmpty()
                //        select new { c.BankAccountId, c.Amount, type = subd.Type ?? string.Empty };
                //var s= q.Sum(o => o.type == "Расходы" ? -o.Amount : o.Amount);

                IQueryable<BankAccountView> views =
                    from c in db.BankAccounts
                    select new BankAccountView
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CardType = c.CardType,
                        Currency = c.Currency,
                        AccountType = c.Cash == CashType.Наличная ? "Наличные" : (c.CardType == CardType.Нет ? "Расчетный счет" : "Банковская карта"),
                        Locking = c.Locking,
                        Payment = c.Payment,
                        Total = c.Total
                    };
                foreach (BankAccountView account in views)
                {
                    var total = query.FirstOrDefault(w => w.Key == account.Id);
                    if (total != null)
                        account.Total += total.Total;
                    CheckingAccount card = new CheckingAccount(account);
                    card.Margin = new Thickness(20);
                    //panel.Children.Add(card);
                    panel.Items.Add(card);
                }
            }
        }
    }
}
