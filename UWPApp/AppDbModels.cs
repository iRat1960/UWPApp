﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Required, Display(Name = "Тип")]
        public CategoriesType Type { get; set; }
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
