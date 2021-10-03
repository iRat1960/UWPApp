using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWPApp
{
    public class HFContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ContentOfOperation> ContentOfOperations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Creditor> Creditors { get; set; }
        public DbSet<AccountingEntry> Entries { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Revolving> Revolvings { get; set; }

        //db.Database.ExecuteSqlCommand("ALTER TABLE dbo.Players ADD CONSTRAINT Players_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE SET NULL");

        public HFContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=HomeFinances.db");
        }
    }

    public sealed class Global
    {
        public static Dictionary<int, string> GlyphList = new Dictionary<int, string>
        {
            { 0, "" },  
            { 1, "\ue811" }, 
            { 2, "\ue82f" }, 
            { 3, "\ue704" }, 
            { 4, "\ue95e" },
            { 5, "\ue7be" }, 
            { 6, "\ue7bf" }, 
            { 7, "\uec09" }, 
            { 8, "\ue72d" }, 
            { 9, "\ue776" },
            { 10, "\ued55" }, 
            { 11, "\ue8a7" }, 
            { 12, "\ue752" }, 
            { 13, "\uf0e3" }, 
            { 14, "\ue715" },
            { 15, "\ue94c" }, 
            { 16, "\uec26" }, 
            { 17, "\uecc5" }, 
            { 18, "\uf6b8" },
            { 19, "\ue944" },
            { 20, "\uf0e4" }, 
            { 21, "\ue709" }, 
            { 22, "\ue70a" }, 
            { 23, "\ue703" }, 
            { 24, "\ue723" },
            { 25, "\ue724" }, 
            { 26, "\ue728" }, 
            { 27, "\ue731" }, 
            { 28, "\ue77f" },
            { 29, "\uec88" },
            { 30, "\uec0a" },
            { 31, "\uec32" },
            { 32, "\uec1b" }
        };
    }

    public class GlyphsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int i = (int)value;
            string ret = Global.GlyphList[i];
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    #region " Перечисления "
    public enum CurrencyType
    {
        RUB,
        USD,
        EUR,
        CNY
    }
    public enum CardType
    {
        [System.ComponentModel.DataAnnotations.Display(Name = " ")]
        Нет = 0,
        Расчётная = 1,
        Кредитная = 2,
        Дебетовая = 3
    }
    public enum PaymentSystem
    {
        [Display(Name = " ")]
        Нет = 0,
        Мир,
        Visa,
        MasterCard,
        AmericanExpress,
        JCB,
        UnionPay
    }
    public enum ChartsType
    {
        [Display(Name = " ")]
        Нет = 0,
        Активный = 1,
        Пассивный = 2,
        АктивноПассивный = 3
    }
    public enum RecordType
    {
        Разделы,
        Счета,
        Субсчета
    }
    public enum CashType
    {
        Наличная = 0,
        Безналичная = 1
    }
    public enum CategoriesType
    {
        Доходы = 0,
        Расходы
    }
    public enum CreditorsType
    {
        Дебитор = 0,
        Кредитор
    }
    #endregion
    
    public class SettingView : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("Id"); }
        }

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; OnPropertyChanged("Caption"); }
        }

        private string _scaption;
        public string SubCaption
        {
            get { return _scaption; }
            set { _scaption = value; OnPropertyChanged("SubCaption"); }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        private bool _selected;
        public bool ItemSelected
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("ItemSelected"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    #region " Таблицы данных "
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BankAccount> BankAccounts { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ContentOfOperation
    {
        public int Id { get; set; }
        [Required, StringLength(250), Display(Name = "Содержание хозяйственной операции")]
        public string OperationName { get; set; }
        [Required, StringLength(6), Display(Name = "Дт")]
        public string Debit { get; set; }
        [Required, StringLength(6), Display(Name = "Кт")]
        public string Credit { get; set; }
    }

    public class Creditor
    {
        public int Id { get; set; }
        [Required, StringLength(100), Display(Name = "Наименование")]
        public string Name { get; set; }
        [Required, Display(Name = "Тип")]
        public CreditorsType Type { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(100), Display(Name = "Наименование")]
        public string Name { get; set; }
        [Required, StringLength(100), Display(Name = "Иконка")]
        public string Icons { get; set; }
        [Required, Display(Name = "Код шрифта")]
        public int Glyphs { get; set; }
        public int SubGlyph { get; set; }
        [Required, Display(Name = "Тип")]
        public string Type { get; set; }
        [Display(Name = "Проводка")]
        public int? ContentOfOperationId { get; set; }
        public int? ParentId { get; set; }
    }
    // Аналитические счета по денежным средствам
    public class BankAccount
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Форма расчета")]
        public CashType Cash { get; set; }
        [Display(Name = "Валюта")]
        public CurrencyType Currency { get; set; }
        [Display(Name = "Тип карты")]
        public CardType CardType { get; set; }
        [Display(Name = "Платёжная система")]
        public PaymentSystem Payment { get; set; }
        [Display(Name = "Начальная дата")]
        public DateTime DateTotal { get; set; }
        [Display(Name = "Сумма на начало")]
        [Range(typeof(decimal), "0,0", "1000000,0", ErrorMessage = "Наименьшая сумма - 0 рублей, в качестве разделителя дробной и целой части используется запятая")]
        public decimal Total { get; set; }
        [Display(Name = "Идентификатор")]
        public int ChartOfAccountId { get; set; }
        [Display(Name = "Заблокирован")]
        public bool Locking { get; set; }
    }
    // Оборотка по денежным средствам
    public class Revolving
    {
        public int Id { get; set; }
        [Display(Name = "Дата операции")]
        public DateTime DateEntry { get; set; }
        [Display(Name = "Сумма")]
        [Range(typeof(decimal), "0,05", "10000000,0", ErrorMessage = "Наименьшая сумма - 0,05 рубля, в качестве разделителя дробной и целой части используется запятая")]
        public decimal Amount { get; set; }
        public CategoriesType Type { get; set; }
    }
    // Бухгалтерские проводки
    public class AccountingEntry
    {
        public int Id { get; set; }
        [Display(Name = "Дата операции")]
        public DateTime DateEntry { get; set; }
        [Display(Name = "Подкатегория")]
        public int CategoryId { get; set; }
        [Display(Name = "Счёт платежа")]
        public int CardsId { get; set; }
        [Display(Name = "Сумма")]
        [Range(typeof(decimal), "0,05", "10000000,0", ErrorMessage = "Наименьшая сумма - 0,05 рубля, в качестве разделителя дробной и целой части используется запятая")]
        public decimal Amount { get; set; }
        [Display(Name = "Валюта")]
        public CurrencyType Currency { get; set; }
        public bool flag { get; set; }

        public Category Category { get; set; }
        public BankAccount Cards { get; set; }
    }
    #endregion
}
