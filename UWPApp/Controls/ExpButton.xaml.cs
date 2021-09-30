using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.Controls
{
    public sealed partial class ExpButton : UserControl
    {
        public string IconSource
        {
            get => image.Source.ToString();
            set => image.Source = new BitmapImage(new Uri("ms-appx:///" + value, UriKind.RelativeOrAbsolute));
        }

        public string TextTop
        {
            get { return textTop.Text; }
            set { textTop.Text = value; }
        }

        public string Comment
        {
            get { return textType.Text; }
            set { textType.Text = value; }
        }

        public bool ButtonVisibility
        {
            get { return sp.Visibility == Visibility.Visible; }
            set { sp.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public ExpButton()
        {
            InitializeComponent();
        }
    }
}
