using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LCARSToolkit.Controls
{
    public sealed partial class Rectangle : UserControl
    {
        public Brush Fill
        {
            get { return rect.Fill; }
            set { SetValue(FillProperty, value); rect.Fill = value; }
        }
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(Rectangle), new PropertyMetadata(null));


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
        public static readonly DependencyProperty IlluminationProperty = DependencyProperty.Register("Illumination", typeof(Illumination), typeof(Rectangle), new PropertyMetadata(Illumination.On));


        public Rectangle()
        {
            this.InitializeComponent();
            Extensions.FlashTimer.Tick += delegate { IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On); };
        }
    }
}
