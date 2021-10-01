using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace UWPApp.Controls
{
    public sealed partial class CategoryControl : UserControl
    {
        //List<CategoryView> Categories;
        ObservableCollection<ListItemData> collection = new ObservableCollection<ListItemData>();
        StandardUICommand deleteCommand;
        XamlUICommand updateCommand;

        public delegate void xListBox_SelectionChanged(object sender, SelectionChangedEventArgs e);

        private xListBox_SelectionChanged SelectionChanged;

        public void RegisterDelegate(xListBox_SelectionChanged select)
        {
            SelectionChanged = select;
        }

        public CategoryControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox.SelectedIndex = 0;
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
            using (HFContext db = new HFContext())
            {
                int ind = comboBox.SelectedIndex;
                List<Category> list;
                if (ind == 0)
                    list = db.Categories.Where(o => o.ParentId == 0).ToList();
                else
                    list = db.Categories.Where(o => o.ParentId == 0 && o.Type == (ind == 1 ? "Доходы" : "Расходы")).ToList();
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
                //IQueryable<CategoryView> q;
                //if (ind == 0)
                //{
                //    q = from c in db.Categories
                //        select new CategoryView
                //        {
                //            Id = c.Id,
                //            Name = c.Name,
                //            Icons = c.Icons,
                //            Type = c.Type,
                //            ItemSelected = false
                //        };
                //}
                //else
                //{
                //    q = from c in db.Categories
                //        where c.Type == (ind == 1 ? "Доходы" : "Расходы")
                //        select new CategoryView
                //        {
                //            Id = c.Id,
                //            Name = c.Name,
                //            Icons = c.Icons,
                //            Type = c.Type,
                //            ItemSelected = false
                //        };
                //}
                //Categories = q.ToList<CategoryView>();
                //list.ItemsSource = Categories;
                count.Text = list.Count.ToString();
            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);

            //if (list.SelectedItem != null)
            //{
            //    CategoryView view = list.SelectedItem as CategoryView;
            //    view.ItemSelected = true;
            //    for (int i = 0; i < list.Items.Count; i++)
            //    {
            //        CategoryView child = (CategoryView)list.Items[i];
            //        if (child.Name != view.Name)
            //        {
            //            child.ItemSelected = false;
            //        }
            //    }
            //}
        }

        private void suggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListRefresh();
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

    public class ListItemData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icons { get; set; }
        public string Type { get; set; }
        public ICommand DelCommand { get; set; }
        public ICommand UpdCommand { get; set; }
    }
}
