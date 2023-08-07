using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LCARSToolkit.Controls
{
    public sealed partial class Stump : UserControl
    {

        public Stump()
        {
            this.InitializeComponent();
            Extensions.FlashTimer.Tick += FlashTimer_Tick;
            Unloaded += (s, e) => { Extensions.FlashTimer.Tick -= FlashTimer_Tick; };
        }

        private void FlashTimer_Tick(object sender, object e)
        {
            IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On);
        }

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
            typeof(Illumination), typeof(Stump), new PropertyMetadata(Illumination.On));

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), 
            typeof(Stump), new PropertyMetadata(.0,PathUpdateCallback));

        private static void PathUpdateCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Stump).UpdatePath();
        }

        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double),
            typeof(Stump), new PropertyMetadata(.0, PathUpdateCallback));

        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), 
            typeof(Stump), new PropertyMetadata(Direction.Right,PathUpdateCallback));
                
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }        
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush),
            typeof(Stump), new PropertyMetadata(null));

        private void UpdatePath()
        {
            string geo = string.Empty;
            CultureInfo formatLanguage = new CultureInfo("en-US");

            switch (Direction)
            {
                case Direction.Right:
                    geo = $"M0,0 l{Length.ToString(formatLanguage)},0 a {(Diameter/2).ToString(formatLanguage)},{(Diameter/2).ToString(formatLanguage)} 180 0 1 0,{Diameter.ToString(formatLanguage)} l {(-Length).ToString(formatLanguage)},0 z";
                    break;
                case Direction.Left:
                    geo = $"M{(Length+Diameter/2).ToString(formatLanguage)},0 l{(-Length).ToString(formatLanguage)},0 a {(Diameter / 2).ToString(formatLanguage)},{(Diameter / 2).ToString(formatLanguage)} 180 0 0 0,{Diameter.ToString(formatLanguage)} l {Length.ToString(formatLanguage)},0 z";
                    break;
                case Direction.Up:
                    geo = $"M0,{(Length+Diameter/2).ToString(formatLanguage)} l0,{(-Length).ToString(formatLanguage)} a {(Diameter / 2).ToString(formatLanguage)},{(Diameter / 2).ToString(formatLanguage)} 180 0 0 {Diameter.ToString(formatLanguage)},0 l 0,{Length.ToString(formatLanguage)} z";
                    break;
                case Direction.Down:
                    geo = $"M0,0 l0,{Length.ToString(formatLanguage)} a {(Diameter / 2).ToString(formatLanguage)},{(Diameter / 2).ToString(formatLanguage)} 180 0 1 {Diameter.ToString(formatLanguage)},0 l 0,{(-Length).ToString(formatLanguage)} z";
                    break;
            }
            path.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
            mask.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
        }
    }
}
