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
        Category category, parentCategory;

        public CategoryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = 0, pid = 0, key = 0;
            string type = "";
            if (e.Parameter != null)
            {
                string args = (string)e.Parameter;
                string[] str = args.Split(',');
                id = Convert.ToInt32(str[0]);
                pid = Convert.ToInt32(str[1]);
                using (HFContext db = new HFContext())
                {
                    category = db.Categories.FirstOrDefault(c => c.Id == id);
                    if (pid > 0)
                    {
                        parentCategory = db.Categories.FirstOrDefault(c => c.Id == pid);
                        if (parentCategory != null)
                        {
                            key = parentCategory.Glyphs;
                            type = parentCategory.Type;
                            icon.Glyph = Global.GlyphList[key];
                            buttonIcon.Visibility = Visibility.Collapsed;
                            borderSub.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            if (category != null)
            {
                headerBlock.Text = "Редактирование ";
                nameBox.Text = category.Name;
                type = category.Type;
                icon.Glyph = Global.GlyphList[category.Glyphs];
            }
            else
            {
                category = new Category { Id = 0, Name = "", Type = type, Glyphs = key, ParentId = pid, ContentOfOperationId = 1 };
            }
            headerBlock.Text += (parentCategory != null ? "под" : "") + "категории";
            if (type.Length > 0)
            {
                comboBox.Visibility = Visibility.Collapsed;
                border.Visibility = Visibility.Visible;
                textBox.Text = category.Type;
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
            using (HFContext db = new HFContext())
            {
                Color color = Color.FromArgb(255, 18, 18, 18);
                foreach (var item in Global.GlyphList)
                {
                    var c = db.Categories.FirstOrDefault(o => o.Glyphs == item.Key);
                    if (c == null)
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
