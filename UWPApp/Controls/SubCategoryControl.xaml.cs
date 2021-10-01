using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class SubCategoryControl : UserControl
    {
        ObservableCollection<ListItemData> collection = new ObservableCollection<ListItemData>();
        StandardUICommand deleteCommand;
        XamlUICommand updateCommand;
        int CategoryId = 0;

        public SubCategoryControl()
        {
            InitializeComponent();
        }

        public SubCategoryControl(int id, string name)
        {
            CategoryId = id;
            InitializeComponent();
            poly.Visibility = Visibility.Visible;
            title.FontSize = 18;
            title.Text = name;
        }

        private void grid1_Loaded(object sender, RoutedEventArgs e)
        {
            deleteCommand = new StandardUICommand(StandardUICommandKind.Delete);
            updateCommand = CustomXamlUICommand;
            deleteCommand.ExecuteRequested += DeleteCommand_ExecuteRequested;
            ListRefresh();
        }

        private void ListRefresh()
        {
            if (CategoryId > 0)
            {
                using (HFContext db = new HFContext())
                {
                    List<Category> list = db.Categories.Where(o => o.ParentId == CategoryId).ToList();
                    collection.Clear();
                    foreach (Category c in list)
                    {
                        collection.Add(new ListItemData
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Icons = c.Icons,
                            Type = c.Type,
                            DelCommand = deleteCommand,
                            UpdCommand = updateCommand
                        });
                    }
                    count.Text = list.Count.ToString();
                }
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewSwipeContainer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }
        
        private void ListViewSwipeContainer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
        }

        private void list_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            listView.ItemsSource = collection;
        }
        
        private void DeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            //if (args.Parameter != null)
            //{
            //    foreach (var i in collection)
            //    {
            //        if (i.Name == (args.Parameter as string))
            //        {
            //            collection.Remove(i);
            //            return;
            //        }
            //    }
            //}
            //if (ListViewRight.SelectedIndex != -1)
            //{
            //    collection.RemoveAt(ListViewRight.SelectedIndex);
            //}
        }

        private void UpdateCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            //if (args.Parameter != null)
            //{
            //    foreach (var i in collection)
            //    {
            //        if (i.Name == (args.Parameter as string))
            //        {
            //            collection.Remove(i);
            //            return;
            //        }
            //    }
            //}
            //if (ListViewRight.SelectedIndex != -1)
            //{
            //    collection.RemoveAt(ListViewRight.SelectedIndex);
            //}
        }
    }
}
