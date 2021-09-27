using System;
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
using System.Collections.ObjectModel;

namespace UWPApp.Controls
{
    public sealed partial class GraphControl : UserControl
    {
        private ObservableCollection<DataItem> _data = new ObservableCollection<DataItem>()
        {
            new DataItem() { Name="Приход", Value=569 },
            new DataItem() { Name="Расход", Value=198 }
        };

        public ObservableCollection<DataItem> Data { get { return _data; } }

        public GraphControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
    }

    public class DataItem
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

}
