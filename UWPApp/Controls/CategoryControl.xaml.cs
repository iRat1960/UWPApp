using System;
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
        ObservableCollection<ListItemData> collection = new ObservableCollection<ListItemData>();
        StandardUICommand deleteCommand;
        XamlUICommand updateCommand;

        public delegate void xListBox_SelectionChanged(object sender, SelectionChangedEventArgs e);
        public delegate void CategoryAdd();

        private xListBox_SelectionChanged selectionChanged;
        private CategoryAdd categoryAdd;

        public void RegisterDelegate(xListBox_SelectionChanged arg)
        {
            selectionChanged = arg;
        }
        public void RegisterDelegate(CategoryAdd arg)
        {
            categoryAdd = arg;
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
                        Glyphs = c.Glyphs,
                        Type = c.Type,
                        DelCommand = deleteCommand,
                        UpdCommand = updateCommand
                    });
                }
                count.Text = list.Count.ToString();
            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChanged?.Invoke(sender, e);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            categoryAdd?.Invoke();
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
        
        private async void DeleteCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            int id = args.Parameter != null ? (int)args.Parameter : -1;
            if (id > -1)
            {
                foreach (var c in collection)
                {
                    if (c.Id == id)
                    {
                        ContentDialog deleteFileDialog = new ContentDialog()
                        {
                            Title = "Подтверждение действия",
                            Content = "Вы действительно хотите удалить категорию " + c.Name + "?",
                            PrimaryButtonText = "ОК",
                            SecondaryButtonText = "Отмена"
                        };
                        ContentDialogResult result = await deleteFileDialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            collection.Remove(c);
                            using (HFContext db = new HFContext())
                            {
                                Category category = db.Categories.FirstOrDefault(o => o.Id == id);
                                if (category != null)
                                {
                                    db.Categories.Remove(category);
                                    db.SaveChanges();
                                }
                            }
                        }
                        return;
                    }
                }
            }
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
        public int Glyphs { get; set; }
        public int SubGlyph { get; set; }
        public string Type { get; set; }
        public ICommand DelCommand { get; set; }
        public ICommand UpdCommand { get; set; }
    }
}
