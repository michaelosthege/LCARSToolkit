using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LCARSToolkit.Controls
{
    public sealed partial class List : UserControl
    {
        public List()
        {
            this.InitializeComponent();
            list.ItemClick += (sender, args) =>
            {
                this.SoundElement?.Play();

                ItemClicked(sender, args);
            };
        }


        public ItemCollection Items => list.Items;

        public IList<object> SelectedItems => list.SelectedItems;


        public Brush ItemFill
        {
            get
            {
                return (Brush) GetValue(ItemFillProperty);
            }
            set
            {
                SetValue(ItemFillProperty, value);
            }
        }

        public MediaElement SoundElement
        {
            get
            {
                return (MediaElement) GetValue(SoundElementProperty);
            }
            set
            {
                SetValue(SoundElementProperty, value);
            }
        }

        public static readonly DependencyProperty SoundElementProperty =
            DependencyProperty.Register(nameof(SoundElement), typeof(MediaElement), typeof(List),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ItemTextColorProperty =
            DependencyProperty.Register(nameof(ItemTextColor), typeof(Brush), typeof(List), new PropertyMetadata(null));

        public Brush ItemTextColor
        {
            get
            {
                return (Brush) GetValue(ItemTextColorProperty);
            }
            set
            {
                SetValue(ItemTextColorProperty, value);
            }
        }

        public static readonly DependencyProperty ItemFillProperty =
            DependencyProperty.Register(nameof(ItemFill), typeof(Brush), typeof(List), new PropertyMetadata(null));

        public event ItemClickEventHandler ItemClicked;
    }
    public class LCARSListItem
    {
        public string Text
        {
            get; set;
        }
        public Illumination Illumination
        {
            get; set;
        }

        public LCARSListItem(string text)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            this.Illumination = Illumination.On;
        }

        public LCARSListItem(string text, Illumination illumination) : this(text) => this.Illumination = illumination;
    }
}
