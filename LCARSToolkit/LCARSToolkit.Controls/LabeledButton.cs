using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LCARSToolkit.Controls.Annotations;

// using Windows.UI;
// using Windows.UI.Xaml;
// using Windows.UI.Xaml.Controls;
// using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace LCARSToolkit.Controls
{
    public sealed class LabeledButton : Button
    {
        public LabeledButton()
        {
            this.DefaultStyleKey = typeof(LabeledButton);

            this.Click += (s, e) =>
            {
                if( s is not LabeledButton button) return;
                Debug.WriteLine($"{button.Content}[{button.Label}] Pressed");
            };
            // this.Click += (s,e) => SoundElement?.Play();
            // subscribe to the single, global FlashTimer such that everything flashes synchronously
            Extensions.FlashTimer.Tick += FlashTimer_Tick;
            this.Unloaded += Button_Unloaded;
        }

        public FlowDirection Direction
        {
            get => (FlowDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(FlowDirection), 
            typeof(LabeledButton), new PropertyMetadata(FlowDirection.LeftToRight, DirectionChanged));
        
        public double SideWidth
        {
            get => (double)GetValue(SideWidthProperty);
            set => SetValue(SideWidthProperty, value);
        }
        public static readonly DependencyProperty SideWidthProperty = DependencyProperty.Register("SideWidth", typeof(double), 
            typeof(LabeledButton), new PropertyMetadata(10.0));
        
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set
            {
                SetValue(LabelProperty, value);
            }
        }
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), 
            typeof(LabeledButton), new PropertyMetadata(string.Empty, LabelChanged));

        private static void LabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not LabeledButton button) return;

            Debug.WriteLine($"UpdateLabel {e.OldValue} => {button.Label}");
            button.LabelVisibility = string.IsNullOrEmpty(button.Label) ? Visibility.Collapsed : Visibility.Visible;
        }

        public double LabelSize
        {
            get => (double)GetValue(LabelSizeProperty);
            set => SetValue(LabelSizeProperty, value);
        }
        public static readonly DependencyProperty LabelSizeProperty = DependencyProperty.Register("LabelSize", typeof(double), 
            typeof(LabeledButton), new PropertyMetadata(60.0));

        public Brush LabelColor
        {
            get => (Brush)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }

        public static readonly DependencyProperty LabelColorProperty = DependencyProperty.Register("LabelColor", typeof(Brush), 
            typeof(LabeledButton), new PropertyMetadata(Brushes.DarkOrange));

        public Visibility LabelVisibility
        {
            get => (Visibility)GetValue(LabelVisibilityProperty);
            set => SetValue(LabelVisibilityProperty, value);
        }

        public static readonly DependencyProperty LabelVisibilityProperty = DependencyProperty.Register("LabelVisibility", typeof(Visibility), 
            typeof(LabeledButton), new PropertyMetadata(Visibility.Collapsed));

        private static void DirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not LabeledButton caller) return;
            if(caller.GetTemplateChild("label") is not FrameworkElement label) return;
            if(caller.GetTemplateChild("side") is not FrameworkElement side) return;

            if (caller.Direction == FlowDirection.LeftToRight)
            {
                if (caller.GetTemplateChild("left") is Stump stump)
                {
                    DockPanel.SetDock(stump, Dock.Left);
                }
                DockPanel.SetDock(side, Dock.Left);
                DockPanel.SetDock(label, Dock.Left);
            }
            else
            {
                DockPanel.SetDock(side, Dock.Right);
                if (caller.GetTemplateChild("right") is Stump stump)
                {
                    Debug.WriteLine("Right to Right");
                    DockPanel.SetDock(stump, Dock.Right);
                }
                DockPanel.SetDock(label, Dock.Right);
            }
        }

        internal void SetMasks(bool isLit)
        {
            try
            {
                (this.GetTemplateChild("mask") as Border).Background = (isLit) ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Color.FromArgb(0x7F, 0x00, 0x00, 0x00));
            }
            catch
            { }            
        }

       private void FlashTimer_Tick(object sender, object e)
        {
            IsLit = (Illumination == Illumination.Flashing) ? !IsLit : (Illumination == Illumination.On);
        }

        private void Button_Unloaded(object sender, RoutedEventArgs e)
        {
            Extensions.FlashTimer.Tick -= FlashTimer_Tick;
        }

        private bool _IsLit;
        public bool IsLit
        {
            get => _IsLit;
            private set
            {
                if (_IsLit != value)
                {
                    _IsLit = value;
                    SetMasks(value);
                }
            }
        }

        public Illumination Illumination
        {
            get => (Illumination)GetValue(IlluminationProperty);
            set => SetValue(IlluminationProperty, value);
        }

        public static readonly DependencyProperty IlluminationProperty = 
            DependencyProperty.Register("Illumination", typeof(Illumination), typeof(LabeledButton), 
                new PropertyMetadata(Illumination.On));

        public Visibility LeftVisibility
        {
            get => (Visibility)GetValue(LeftVisibilityProperty);
            set => SetValue(LeftVisibilityProperty, value);
        }

        public static readonly DependencyProperty LeftVisibilityProperty = 
            DependencyProperty.Register("LeftVisibility", typeof(Visibility), typeof(LabeledButton), 
                new PropertyMetadata(Visibility.Visible));

        public Visibility RightVisibility
        {
            get => (Visibility)GetValue(RightVisibilityProperty);
            set => SetValue(RightVisibilityProperty, value);
        }

        public static readonly DependencyProperty RightVisibilityProperty = 
            DependencyProperty.Register("RightVisibility", typeof(Visibility), typeof(LabeledButton), 
                new PropertyMetadata(Visibility.Visible));

        public Stumps Stumps
        {
            get => (Stumps)GetValue(StumpsProperty);
            set => SetValue(StumpsProperty, value);
        }

        public static readonly DependencyProperty StumpsProperty = 
            DependencyProperty.Register("Stumps", typeof(Stumps), typeof(LabeledButton), 
                new PropertyMetadata(Stumps.Both, StumpsChanged));

        private static void StumpsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not LabeledButton button) return;

            Debug.WriteLine($"UpdateStumps {e.OldValue} => {button.Stumps}");
            button.LeftVisibility =
                button.Stumps is Stumps.Both or Stumps.Left ? Visibility.Visible : Visibility.Collapsed;
            button.RightVisibility =
                button.Stumps is Stumps.Both or Stumps.Right ? Visibility.Visible : Visibility.Collapsed;
        }

        public Corner ContentAlignment
        {
            get => (Corner)GetValue(ContentAlignmentProperty);
            set => SetValue(ContentAlignmentProperty, value);
        }

        public static readonly DependencyProperty ContentAlignmentProperty = 
            DependencyProperty.Register("ContentAlignment", typeof(Corner), typeof(LabeledButton), new PropertyMetadata(Corner.BottomLeft,
                ContentAlignmentChanged));

        private static void ContentAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not LabeledButton button) return;
            var value = button.ContentAlignment;
            button.HorizontalContentAlignment = (value == Corner.TopLeft || value == Corner.BottomLeft) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            button.VerticalContentAlignment = (value == Corner.TopLeft || value == Corner.TopRight) ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = 
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(LabeledButton), 
                new PropertyMetadata(null));

        public MediaElement SoundElement
        {
            get => (MediaElement)GetValue(SoundElementProperty);
            set => SetValue(SoundElementProperty, value);
        }
        public static readonly DependencyProperty SoundElementProperty = 
            DependencyProperty.Register(nameof(SoundElement), typeof(MediaElement), typeof(LabeledButton), 
                new PropertyMetadata(null));
    }
}
