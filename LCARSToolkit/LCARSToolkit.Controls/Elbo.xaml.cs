﻿using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

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
            typeof(Elbo), new PropertyMetadata(null,UpdatePathCallback));

        public double Column
        {
            get { return (double)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", 
            typeof(double), typeof(Elbo), new PropertyMetadata(null,UpdatePathCallback));

        public double InnerArcRadius
        {
            get { return (double)GetValue(InnerArcRadiusProperty); }
            set { SetValue(InnerArcRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerArcRadiusProperty = DependencyProperty.Register("InnerArcRadius", typeof(double), 
            typeof(Elbo), new PropertyMetadata(null,UpdatePathCallback));

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
            double big = InnerArcRadius + Math.Min(Bar, Column);
            double small = InnerArcRadius;

            string geo = string.Empty;

            CultureInfo formatLanguage = new CultureInfo("en-US");
            // Geometry syntax ()
            // m = start(x,y)
            // a = arc(sizeXY, angle, isLargerThan180, sweepDirection, endXY)
            // l = line(x, y)
            // z = end
            switch (Corner)
            {
                case Corner.TopLeft:
                    geo = $"M0,{(Bar+small).ToString(formatLanguage)} l{Column.ToString(formatLanguage)},0 a {small.ToString(formatLanguage)},{small.ToString(formatLanguage)} 90 0 1 {small.ToString(formatLanguage)},{(-small).ToString(formatLanguage)} l 0,{(-Bar).ToString(formatLanguage)} l {(-Column - small + big).ToString(formatLanguage)},0 a {big.ToString(formatLanguage)},{big.ToString(formatLanguage)} 90 0 0 {(-big).ToString(formatLanguage)},{big.ToString(formatLanguage)} z";
                    break;
                case Corner.TopRight:
                    geo = $"M{(Column + small).ToString(formatLanguage)},{(Bar+small).ToString(formatLanguage)} l{(-Column).ToString(formatLanguage)},0 a {small.ToString(formatLanguage)},{small.ToString(formatLanguage)} 90 0 0 {(-small).ToString(formatLanguage)},{(-small).ToString(formatLanguage)} l 0,{(-Bar).ToString(formatLanguage)} l {(Column + small - big).ToString(formatLanguage)},0 a {big.ToString(formatLanguage)},{big.ToString(formatLanguage)} 90 0 1 {big.ToString(formatLanguage)},{big.ToString(formatLanguage)} z";
                    break;
                case Corner.BottomRight:
                    geo = $"M{(Column+small).ToString(formatLanguage)},0 l{(-Column).ToString(formatLanguage)},0 a {small.ToString(formatLanguage)},{small.ToString(formatLanguage)} 90 0 1 {(-small).ToString(formatLanguage)},{small.ToString(formatLanguage)} l 0,{Bar.ToString(formatLanguage)} l {(Column + small - big).ToString(formatLanguage)},0 a {big.ToString(formatLanguage)},{big.ToString(formatLanguage)} 90 0 0 {big.ToString(formatLanguage)},{(-big).ToString(formatLanguage)} z";
                    break;
                case Corner.BottomLeft:
                    geo = $"M0,0 l{Column.ToString(formatLanguage)},0 a {small.ToString(formatLanguage)},{small.ToString(formatLanguage)} 90 0 0 {small.ToString(formatLanguage)},{small.ToString(formatLanguage)} l 0,{Bar.ToString(formatLanguage)} l {(-Column - small + big).ToString(formatLanguage)},0 a {big.ToString(formatLanguage)},{big.ToString(formatLanguage)} 90 0 1 {(-big).ToString(formatLanguage)},{(-big).ToString(formatLanguage)} z";
                    break;
            }
            path.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
            mask.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
        }
    }
}
