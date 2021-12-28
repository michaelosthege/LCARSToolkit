using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

// using Windows.UI;
// using Windows.UI.Xaml;
// using Windows.UI.Xaml.Controls;
// using Windows.UI.Xaml.Markup;
// using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LCARSToolkit.Controls
{
    public sealed partial class Elbo : UserControl
    {

        public Elbo()
        {
            this.InitializeComponent();
            this.DataContext = this;
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
            typeof(Illumination), typeof(Elbo), new PropertyMetadata(Illumination.On));

        public double Bar
        {
            get { return (double)GetValue(BarProperty); }
            set { SetValue(BarProperty, value); }
        }

        public static readonly DependencyProperty BarProperty = DependencyProperty.Register("Bar", typeof(double), 
            typeof(Elbo), new PropertyMetadata(0.0,UpdatePathCallback));

        public double Column
        {
            get { return (double)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", 
            typeof(double), typeof(Elbo), new PropertyMetadata(0.0,UpdatePathCallback));

        public double InnerArcRadius
        {
            get { return (double)GetValue(InnerArcRadiusProperty); }
            set { SetValue(InnerArcRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerArcRadiusProperty = DependencyProperty.Register("InnerArcRadius", typeof(double), 
            typeof(Elbo), new PropertyMetadata(0.0,UpdatePathCallback));

        public Corner Corner
        {
            get { return (Corner)GetValue(CornerProperty); }
            set { SetValue(CornerProperty, value); }
        }

        public static readonly DependencyProperty CornerProperty = DependencyProperty.Register("Corner", typeof(Corner), 
            typeof(Elbo), new PropertyMetadata(Corner.TopLeft,UpdatePathCallback));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), 
            typeof(Elbo), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        private static void UpdatePathCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Elbo).UpdatePath();
        }

        private void UpdatePath()
        {
            var big = InnerArcRadius + Math.Min(Bar, Column);
            var small = InnerArcRadius;
            var height = Math.Max(big, Bar + small);
            var width = Math.Max(big, Column + small);

            var figure = new PathFigure();
            path.Data = new PathGeometry
            {
                Figures = {figure}
            };
            mask.Data = new PathGeometry
            {
                Figures = {figure}
            };

            var points = Corner switch
            {
                Corner.TopLeft => new[]
                {
                    new Point(0, big),
                    new Point(0, height),
                    new Point(Column, height),
                    new Point(Column, small + Bar),
                    new Point(Column + small, Bar),
                    new Point(width, Bar),
                    new Point(width, 0),
                    new Point(big, 0)
                },
                Corner.TopRight => new[]
                {
                    new Point(width - big, 0),
                    new Point(0, 0),
                    new Point(0, Bar),
                    new Point(width - (Column + small), Bar),
                    new Point(width - Column, Bar + small),
                    new Point(width - Column, height),
                    new Point(width, height),
                    new Point(width, big)
                },
                Corner.BottomRight => new[]
                {
                    new Point(width, height - big),
                    new Point(width, 0),
                    new Point(width - Column, 0),
                    new Point(width - Column, height - (Bar + small)),
                    new Point(width - (Column + small), height - Bar),
                    new Point(0, height - Bar),
                    new Point(0, height),
                    new Point(width - big, height)
                },
                Corner.BottomLeft => new[]
                {
                    new Point(big, height),
                    new Point(width, height),
                    new Point(width, height - Bar),
                    new Point(Column + small, height - Bar),
                    new Point(Column, height - (Bar + small)),
                    new Point(Column, 0),
                    new Point(0, 0),
                    new Point(0, height - big)
                },
                _ => Array.Empty<Point>()
            };

            if (!points.Any()) return;

            figure.StartPoint = points[0];
            figure.Segments.Add(new LineSegment {Point = points[1]});
            figure.Segments.Add(new LineSegment {Point = points[2]});
            figure.Segments.Add(new LineSegment {Point = points[3]});
            figure.Segments.Add(new ArcSegment
            {
                Size = new Size(small, small),
                Point = points[4],
                SweepDirection = SweepDirection.Clockwise
            });
            figure.Segments.Add(new LineSegment {Point = points[5]});
            figure.Segments.Add(new LineSegment {Point = points[6]});
            figure.Segments.Add(new LineSegment {Point = points[7]});
            figure.Segments.Add(new ArcSegment
            {
                Size = new Size(big, big), 
                Point = points[0],
                SweepDirection = SweepDirection.Counterclockwise
            });
        }
    }
}
