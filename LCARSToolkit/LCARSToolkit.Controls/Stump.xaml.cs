// using Windows.UI.Xaml;
// using Windows.UI.Xaml.Controls;
// using Windows.UI.Xaml.Markup;
// using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

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
            var figure = new PathFigure();
            path.Data = new PathGeometry
            {
                Figures = {figure}
            };
            mask.Data = new PathGeometry
            {
                Figures = {figure}
            };
            switch (Direction)
            {
                case Direction.Right:
                    figure.StartPoint = new Point(0, 0);
                    figure.Segments.Add(new LineSegment{Point = new Point(Length, 0)});
                    figure.Segments.Add(new ArcSegment
                    {
                        Size = new Size(Diameter/2, Diameter/2),
                        Point = new Point(Length, Diameter),
                        SweepDirection = SweepDirection.Clockwise
                    });
                    figure.Segments.Add(new LineSegment{Point = new Point(0, Diameter)});
                    figure.Segments.Add(new LineSegment{Point = new Point(0, 0)});
                    break;
                case Direction.Left:
                    figure.StartPoint = new Point(Length + Diameter/2, 0);
                    figure.Segments.Add(new LineSegment{Point = new Point(Diameter/2, 0)});
                    figure.Segments.Add(new ArcSegment
                    {
                        Size = new Size(Diameter/2, Diameter/2),
                        Point = new Point(Diameter/2, Diameter),
                        SweepDirection = SweepDirection.Counterclockwise
                    });
                    figure.Segments.Add(new LineSegment{Point = new Point(Length + Diameter/2, Diameter)});
                    figure.Segments.Add(new LineSegment{Point = new Point(Length + Diameter/2, 0)});
                    break;
                case Direction.Up:
                    figure.StartPoint = new Point(0, Length + Diameter/2);
                    figure.Segments.Add(new LineSegment{Point = new Point(0, Diameter/2)});
                    figure.Segments.Add(new ArcSegment
                    {
                        Size = new Size(Diameter/2, Diameter/2),
                        Point = new Point(Diameter, Diameter/2),
                        SweepDirection = SweepDirection.Clockwise
                    });
                    figure.Segments.Add(new LineSegment{Point = new Point(Diameter, Length + Diameter/2)});
                    figure.Segments.Add(new LineSegment{Point = new Point(0, Length + Diameter/2)});
                    break;
                case Direction.Down:
                    figure.StartPoint = new Point(0, 0);
                    figure.Segments.Add(new LineSegment{Point = new Point(0, Length)});
                    figure.Segments.Add(new ArcSegment
                    {
                        Size = new Size(Diameter/2, Diameter/2),
                        Point = new Point(Diameter, Length),
                        SweepDirection = SweepDirection.Counterclockwise
                    });
                    figure.Segments.Add(new LineSegment{Point = new Point(Diameter, 0)});
                    figure.Segments.Add(new LineSegment{Point = new Point(0, 0)});
                    break;
            }
        }
    }
}
