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
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LCARSToolkit.Controls
{
    public sealed partial class Elbo : UserControl
    {
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
        public static readonly DependencyProperty IlluminationProperty = DependencyProperty.Register("Illumination", typeof(Illumination), typeof(Elbo), new PropertyMetadata(Illumination.On));

        public double Bar
        {
            get { return (double)GetValue(BarProperty); }
            set
            {
                SetValue(BarProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty BarProperty = DependencyProperty.Register("Bar", typeof(double), typeof(Elbo), new PropertyMetadata(null));


        public double Column
        {
            get { return (double)GetValue(ColumnProperty); }
            set
            {
                SetValue(ColumnProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", typeof(double), typeof(Elbo), new PropertyMetadata(null));


        public double InnerArcRadius
        {
            get { return (double)GetValue(InnerArcRadiusProperty); }
            set
            {
                SetValue(InnerArcRadiusProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty InnerArcRadiusProperty = DependencyProperty.Register("InnerArcRadius", typeof(double), typeof(Elbo), new PropertyMetadata(null));

        public Corner Corner
        {
            get { return (Corner)GetValue(CornerProperty); }
            set
            {
                SetValue(CornerProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty CornerProperty = DependencyProperty.Register("Corner", typeof(Corner), typeof(Elbo), new PropertyMetadata(Corner.TopLeft));


        public Brush Fill
        {
            get { return path.Fill; }
            set { path.Fill = value; }
        }
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(Elbo), new PropertyMetadata(null));



        public Elbo()
        {
            this.InitializeComponent();
            this.DataContext = this;
            Extensions.FlashTimer.Tick += delegate { IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On); };
        }

        private void UpdatePath()
        {
            double big = InnerArcRadius + Math.Min(Bar, Column);
            double small = InnerArcRadius;

            string geo = string.Empty;
            // Geometry syntax ()
            // m = start(x,y)
            // a = arc(sizeXY, angle, isLargerThan180, sweepDirection, endXY)
            // l = line(x, y)
            // z = end
            switch (Corner)
            {
                case Corner.TopLeft:
                    geo = $"M0,{Bar+small} l{Column},0 a {small},{small} 90 0 1 {small},{-small} l 0,{-Bar} l {-Column - small + big},0 a {big},{big} 90 0 0 {-big},{big} z";
                    break;
                case Corner.TopRight:
                    geo = $"M{Column + small},{Bar + small} l{-Column},0 a {small},{small} 90 0 0 {-small},{-small} l 0,{-Bar} l {Column + small - big},0 a {big},{big} 90 0 1 {big},{big} z";
                    break;
                case Corner.BottomRight:
                    geo = $"M{Column+small},0 l{-Column},0 a {small},{small} 90 0 1 {-small},{small} l 0,{Bar} l {Column + small - big},0 a {big},{big} 90 0 0 {big},{-big} z";
                    break;
                case Corner.BottomLeft:
                    geo = $"M0,0 l{Column},0 a {small},{small} 90 0 0 {small},{small} l 0,{Bar} l {-Column - small + big},0 a {big},{big} 90 0 1 {-big},{-big} z";
                    break;
            }
            path.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
            mask.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
        }
    }
}
