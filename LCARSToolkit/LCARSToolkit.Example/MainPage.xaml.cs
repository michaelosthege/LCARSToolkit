using LCARSToolkit.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

// using System.Runtime.InteropServices.WindowsRuntime;
// using TCD.Controls;
// using Windows.Foundation;
// using Windows.Foundation.Collections;
// using Windows.UI.Xaml;
// using Windows.UI.Xaml.Controls;
// using Windows.UI.Xaml.Controls.Primitives;
// using Windows.UI.Xaml.Data;
// using Windows.UI.Xaml.Input;
// using Windows.UI.Xaml.Media;
// using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LCARSToolkit.Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Window
    {
        public MainPage()
        {
            this.InitializeComponent();
            SetupEnumCombo(cb1,FlowDirection.LeftToRight);
            SetupEnumCombo(cb2, Stumps.Both);
            SetupEnumCombo(cb3, Illumination.On);
        }
        
        private void SetupEnumCombo<TEnum>(ComboBox box, TEnum defaultVal) where TEnum : struct
        {
            foreach (var val in Enum.GetValues(typeof(TEnum)))
            {
                box.Items.Add(val);
            }

            box.SelectedItem = defaultVal;
        }
    }
}
