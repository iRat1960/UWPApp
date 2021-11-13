using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
