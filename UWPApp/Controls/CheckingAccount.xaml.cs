﻿using System;
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
    public sealed partial class CheckingAccount : UserControl
    {
        BankAccountView account;
        public CheckingAccount()
        {
            InitializeComponent();
        }

        public CheckingAccount(BankAccountView arg)
        {
            account = arg;
            InitializeComponent();
            DataContext = account;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
