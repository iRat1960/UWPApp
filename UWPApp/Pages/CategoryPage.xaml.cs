using System;
using System.Collections.Generic;
using System.Linq;
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //var families = Fonts.GetFontFamilies(@"C:\WINDOWS\Fonts\Arial.TTF");
            //var families = Fonts.SystemFontFamilies;
            //foreach (System.Windows.Media.FontFamily family in families)
            //{
            //    var typefaces = family.GetTypefaces();
            //    foreach (Typeface typeface in typefaces)
            //    {
            //        GlyphTypeface glyph;
            //        typeface.TryGetGlyphTypeface(out glyph);
            //        IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;

            //        foreach (KeyValuePair<int, ushort> kvp in characterMap)
            //        {
            //            Console.WriteLine(String.Format("{0}:{1}", kvp.Key, kvp.Value));
            //        }

            //    }
            //}
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
                if (category != null)
                {
                    category.Name = nameBox.Text;
                    db.Categories.Update(category);
                }
                else
                {
                    db.Categories.Add(new Category { Name = nameBox.Text });
                }
                db.SaveChanges();
            }
            GoToMainPage();
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
