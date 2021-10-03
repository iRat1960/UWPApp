using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPApp.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPApp.Pages;

namespace UWPApp
{
    public sealed partial class SettingPage : Page
    {
        SettingView view;

        public SettingPage()
        {
            view = new SettingView { Caption = "Категории", SubCaption = "Подкатегории" };
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

        private void listCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = (ListView)sender;
            if (list.SelectedItem != null)
            {
                foreach (var child in grid.Children)
                {
                    if (child.GetType().Equals(typeof(SubCategoryControl)))
                        grid.Children.Remove(child);
                }
                ListItemData data = (ListItemData)list.SelectedItem;
                SubCategoryControl cont = new SubCategoryControl(data.Id, data.Name);
                if (cont != null)
                {
                    grid.Children.Add(cont);
                    Grid.SetColumn(cont, 1);
                    Grid.SetRow(cont, 1);
                }
            }
        }
        
        private void CategoryAdd()
        {
            Frame.Navigate(typeof(CategoryPage));
        }

        private void menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menu.SelectedItems != null)
            {
                int ind = menu.SelectedIndex;
                var item = menu.SelectedItem;
                string text = (((item as ListBoxItem).Content as StackPanel).Children[1] as TextBlock).Text;
                view.Caption = text;
                view.SubCaption = "";
                int ct = grid.Children.Count - 1;
                for (int i = ct; i > 0; i--)
                {
                    if (grid.Children[i].GetType().Equals(typeof(CategoryControl)) || grid.Children[i].GetType().Equals(typeof(SubCategoryControl)))
                        grid.Children.RemoveAt(i);
                }
                dynamic cont = null;
                switch (ind)
                {
                    case 0:
                        cont = new CategoryControl();
                        (cont as CategoryControl).RegisterDelegate(listCategory_SelectionChanged);
                        (cont as CategoryControl).RegisterDelegate(CategoryAdd);
                        view.SubCaption = "Подкатегории";
                        SubCategoryControl sub = new SubCategoryControl();
                        grid.Children.Add(sub);
                        Grid.SetColumn(sub, 1);
                        Grid.SetRow(sub, 1);
                        break;
                    case 1:
                        //cont = new CategoryControl();
                        break;
                    case 2:
                        //cont = new CategoryControl();
                        break;
                    case 3:
                        //cont = new CategoryControl();
                        break;
                }
                if (cont != null)
                {
                    grid.Children.Add(cont);
                    Grid.SetColumn(cont, 0);
                    Grid.SetRow(cont, 1);
                }
            }
        }
    }
}
