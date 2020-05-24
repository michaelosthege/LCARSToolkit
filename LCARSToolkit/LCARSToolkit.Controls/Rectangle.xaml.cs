using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LCARSToolkit.Controls
{
    public sealed partial class Rectangle : UserControl
    {

        public Rectangle()
        {
            this.InitializeComponent();
            Extensions.FlashTimer.Tick += FlashTimer_Tick;
            Unloaded += (s, e) => { Extensions.FlashTimer.Tick -= FlashTimer_Tick; };
        }

        private void FlashTimer_Tick(object sender, object e)
        {
            IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On);
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value);}
        }

        public static readonly DependencyProperty FillProperty = 
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(Rectangle), new PropertyMetadata(null));

        private bool _IsLit;
        public bool IsLit
        {
            get
            {
                return _IsLit;
            }
            private set
            {
                if (_IsLit != value)
                {
                    mask.Visibility = (value) ? Visibility.Collapsed : Visibility.Visible;
                    _IsLit = value;
                }
            }
        }

        public Illumination Illumination
        {
            get { return (Illumination)GetValue(IlluminationProperty); }
            set { SetValue(IlluminationProperty, value); }
        }

        public static readonly DependencyProperty IlluminationProperty = DependencyProperty.Register("Illumination", 
            typeof(Illumination), typeof(Rectangle), new PropertyMetadata(Illumination.On));



    }
}
