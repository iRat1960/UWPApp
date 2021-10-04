using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWPApp.Pages
{
    public sealed partial class CategoryPage : Page
    {
        Category category;

        public CategoryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                int id = (int)e.Parameter;
                using (HFContext db = new HFContext())
                {
                    category = db.Categories.FirstOrDefault(c => c.Id == id);
                }
            }
            if (category != null)
            {
                headerBlock.Text = "Редактирование категории";
                nameBox.Text = category.Name;
                comboBox.Visibility = Visibility.Collapsed;
                border.Visibility = Visibility.Visible;
                textBox.Text = category.Type;
                int iconId = category.Glyphs;
                icon.Glyph = Global.GlyphList[iconId];
            }
            else
            {
                category = new Category { Id = 0, Name = "", Type = "", Glyphs = 0, ParentId = 0, ContentOfOperationId = 1 };
            }
        }

        private void GoToMainPage()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            else
                Frame.Navigate(typeof(SettingPage));
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            using (HFContext db = new HFContext())
            {
                category.Name = nameBox.Text;
                if (category.Id != 0)
                {
                    db.Categories.Update(category);
                }
                else
                {
                    category.Type = comboBox.SelectedValue.ToString();
                    db.Categories.Add(category);
                }
                db.SaveChanges();
            }
            GoToMainPage();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dialogBox.Hide();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Color color = Color.FromArgb(255, 18, 18, 18);
            foreach (var item in Global.GlyphList)
            {
                FontIcon ficon = new FontIcon
                {
                    FontSize = 36,
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Glyph = item.Value
                };
                Button b = new Button
                {
                    Background = new SolidColorBrush(color),
                    Height = 64,
                    Width = 64,
                    CornerRadius = new CornerRadius(6),
                    Margin = new Thickness(4),
                    Tag = item.Key
                };
                b.Content = ficon;
                b.Click += button_Click;
                wrap.Children.Add(b);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button b =(sender as Button);
            int key = (int)b.Tag;
            category.Glyphs = key;
            icon.Glyph = Global.GlyphList[key];
            dialogBox.Hide();
        }
    }
}
