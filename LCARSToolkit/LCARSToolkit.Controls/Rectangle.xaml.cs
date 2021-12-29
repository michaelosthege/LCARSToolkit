using System.Windows;
using System.Windows.Media;

namespace LCARSToolkit.Controls
{
    public sealed partial class Rectangle
    {
        public Rectangle()
        {
            this.InitializeComponent();
            Extensions.FlashTimer.Tick += FlashTimer_Tick;
            Unloaded += (_, _) => { Extensions.FlashTimer.Tick -= FlashTimer_Tick; };
        }

        private void FlashTimer_Tick(object sender, object e)
        {
            IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On);
        }

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty FillProperty = 
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(Rectangle), new PropertyMetadata(null));

        public Visibility MaskVisibility
        {
            get => (Visibility)GetValue(MaskVisibilityProperty);
            set => SetValue(MaskVisibilityProperty, value);
        }

        public static readonly DependencyProperty MaskVisibilityProperty = 
            DependencyProperty.Register(nameof(MaskVisibility), typeof(Visibility), typeof(Rectangle), new PropertyMetadata(Visibility.Collapsed));
            
        private bool _isLit;
        public bool IsLit
        {
            get => _isLit;
            private set
            {
                if (_isLit != value)
                {
                    MaskVisibility = (value) ? Visibility.Collapsed : Visibility.Visible;
                    _isLit = value;
                }
            }
        }

        public Illumination Illumination
        {
            get => (Illumination)GetValue(IlluminationProperty);
            set => SetValue(IlluminationProperty, value);
        }

        public static readonly DependencyProperty IlluminationProperty = DependencyProperty.Register("Illumination", 
            typeof(Illumination), typeof(Rectangle), new PropertyMetadata(Illumination.On));
    }
}
