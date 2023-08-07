using LCARSToolkit.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using TCD.Controls;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LCARSToolkit.Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            cb1.SetUpItems(FlowDirection.LeftToRight);
            cb2.SetUpItems(Stumps.Both);
            cb3.SetUpItems(Illumination.On);
            lst.Items.Add(new LCARSListItem("Item 1"));
            lst.Items.Add(new LCARSListItem("Flashing Item 2", Illumination.Flashing));
            lst.Items.Add(new LCARSListItem("Item 3"));
        }

        private void lst_ItemClicked(object sender, ItemClickEventArgs e)
        {
            var item = (e.ClickedItem as LCARSListItem);
            new MessageDialog($"You clicked {item.Text}!").ShowAsync();
        }
    }
}
