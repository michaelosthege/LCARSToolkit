using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class Stump : UserControl
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
        public static readonly DependencyProperty IlluminationProperty = DependencyProperty.Register("Illumination", typeof(Illumination), typeof(Stump), new PropertyMetadata(Illumination.On));



        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set
            {
                SetValue(DiameterProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), typeof(Elbo), new PropertyMetadata(.0));


        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set
            {
                SetValue(LengthProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Elbo), new PropertyMetadata(.0));


        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set
            {
                SetValue(DirectionProperty, value);
                UpdatePath();
            }
        }
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), typeof(Elbo), new PropertyMetadata(Direction.Right));

        
        public Brush Fill
        {
            get { return path.Fill; }
            set { path.Fill = value; }
        }
        
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(Elbo), new PropertyMetadata(null));


        public Stump()
        {
            this.InitializeComponent();
            Extensions.FlashTimer.Tick += delegate { IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On); };
        }

        private void UpdatePath()
        {
            string geo = string.Empty;

            switch (Direction)
            {
                case Direction.Right:
                    geo = $"M0,0 l{Length},0 a {Diameter/2},{Diameter/2} 180 0 1 0,{Diameter} l {-Length},0 z";
                    break;
                case Direction.Left:
                    geo = $"M{Length+Diameter/2},0 l{-Length},0 a {Diameter / 2},{Diameter / 2} 180 0 0 0,{Diameter} l {Length},0 z";
                    break;
                case Direction.Up:
                    geo = $"M0,{Length+Diameter/2} l0,{-Length} a {Diameter / 2},{Diameter / 2} 180 0 0 {Diameter},0 l 0,{Length} z";
                    break;
                case Direction.Down:
                    geo = $"M0,0 l0,{Length} a {Diameter / 2},{Diameter / 2} 180 0 1 {Diameter},0 l 0,{-Length} z";
                    break;
            }
            path.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
            mask.Data = (Geometry)XamlReader.Load($"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>{geo}</Geometry>");
        }
    }
}
