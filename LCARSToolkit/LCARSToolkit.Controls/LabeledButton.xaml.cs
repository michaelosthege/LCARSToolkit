using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SButton = System.Windows.Controls.Button;

namespace LCARSToolkit.Controls
{
    public partial class LabeledButton : Button
    {
        public LabeledButton()
        {
            InitializeComponent();
        }
        
        public FlowDirection Direction
        {
            get { return (FlowDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(FlowDirection), 
            typeof(LabeledButton), new PropertyMetadata(FlowDirection.LeftToRight, DirectionChanged));
        
        public double SideWidth
        {
            get { return (double)GetValue(SideWidthProperty); }
            set { SetValue(SideWidthProperty, value); }
        }
        public static readonly DependencyProperty SideWidthProperty = DependencyProperty.Register("SideWidth", typeof(double), 
            typeof(LabeledButton), new PropertyMetadata(100.0));
        
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), 
            typeof(LabeledButton), new PropertyMetadata(string.Empty));

        private static void DirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LabeledButton).SetColumns();
            (d as LabeledButton).UpdateCorners();
        }
        
        private void SetColumns()
        {
            var starAuto = new GridLength[] { new GridLength(1, GridUnitType.Star), GridLength.Auto };
            var autoStar = starAuto.Reverse();
            switch (Direction)
            {
                case FlowDirection.RightToLeft:
                    SetColumnDefinitions((GetTemplateChild("root") as Grid)?.ColumnDefinitions, starAuto);
                    SetColumnDefinitions((GetTemplateChild("side") as Grid).ColumnDefinitions, autoStar);
                    Grid.SetColumn((GetTemplateChild("rect") as FrameworkElement), 1);
                    Grid.SetColumn((GetTemplateChild("viewbox") as FrameworkElement), 0);
                    Grid.SetColumn((GetTemplateChild("side") as FrameworkElement), 1);
                    Grid.SetColumn((GetTemplateChild("RootGrid") as FrameworkElement), 0);
                    break;
                case FlowDirection.LeftToRight:
                    SetColumnDefinitions((GetTemplateChild("root") as Grid).ColumnDefinitions, autoStar);
                    SetColumnDefinitions((GetTemplateChild("side") as Grid).ColumnDefinitions, starAuto);
                    Grid.SetColumn((GetTemplateChild("rect") as FrameworkElement), 0);
                    Grid.SetColumn((GetTemplateChild("viewbox") as FrameworkElement), 1);
                    Grid.SetColumn((GetTemplateChild("side") as FrameworkElement), 0);
                    Grid.SetColumn((GetTemplateChild("RootGrid") as FrameworkElement), 1);
                    break;
            }
        }

        private void SetColumnDefinitions(ColumnDefinitionCollection coldefs, IEnumerable<GridLength> widths)
        {
            coldefs.Clear();
            foreach (var item in widths)
                coldefs.Add(new ColumnDefinition() { Width = item });
        }

        internal void UpdateCorners()
        {
            var radius = this.ActualHeight / 2;
            CornerRadius roundRight = new CornerRadius(0, radius, radius, 0);
            CornerRadius roundLeft = new CornerRadius(radius, 0, 0, radius);
            CornerRadius notRound = new CornerRadius(0);
             
            Thickness padLeft = new Thickness(radius,0,5,0);
            Thickness padRight = new Thickness(5,0,radius,0);
            Thickness noPad = new Thickness(5,0,5,0);
            
            switch (Stumps)
            {
                // case Stumps.None:
                //     // this.CornerRadius = new CornerRadius(0);
                //     this.Padding = new Thickness(5, 0, 5, 0);
                //     (GetTemplateChild("rect") as Border).CornerRadius = new CornerRadius(0);
                //     break;
                // case Stumps.Left:
                //     (GetTemplateChild("rect") as Border).CornerRadius = (Direction == FlowDirection.LeftToRight) ? roundLeft : notRound;
                //     // this.CornerRadius = (Direction == FlowDirection.LeftToRight) ? notRound : roundLeft;
                //     this.Padding = (Direction == FlowDirection.RightToLeft) ? padLeft : noPad;
                //     break;
                // case Stumps.Right:
                //     (GetTemplateChild("rect") as Border).CornerRadius = (Direction == FlowDirection.RightToLeft) ? roundRight : notRound;
                //     // this.CornerRadius = (Direction == FlowDirection.RightToLeft) ? notRound : roundRight;
                //     this.Padding = (Direction == FlowDirection.LeftToRight) ? padRight : noPad;
                //     break;
                // case Stumps.Both:
                //     (GetTemplateChild("rect") as Border).CornerRadius = (Direction == FlowDirection.LeftToRight) ? roundLeft : roundRight;
                //     // this.CornerRadius = (Direction == FlowDirection.LeftToRight) ? roundRight : roundLeft;
                //     this.Padding = (Direction == FlowDirection.LeftToRight) ? padRight : padLeft;
                //     break;
            }
        }

        internal void SetMasks(bool isLit)
        {
            try
            {
                (this.GetTemplateChild("mask2") as Rectangle).Fill = (isLit) ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Color.FromArgb(0x7F, 0x00, 0x00, 0x00));
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
            get
            {
                return _IsLit;
            }
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
            get { return (Illumination)GetValue(IlluminationProperty); }
            set { SetValue(IlluminationProperty, value); }
        }

        public static readonly DependencyProperty IlluminationProperty = 
            DependencyProperty.Register("Illumination", typeof(Illumination), typeof(Button), 
                new PropertyMetadata(Illumination.On));

        public Stumps Stumps
        {
            get { return (Stumps)GetValue(StumpsProperty); }
            set { SetValue(StumpsProperty, value); }
        }

        public static readonly DependencyProperty StumpsProperty = 
            DependencyProperty.Register("Stumps", typeof(Stumps), typeof(Button), 
                new PropertyMetadata(Stumps.Both, StumpsChanged));

        private static void StumpsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LabeledButton).UpdateCorners();
        }

        public Corner ContentAlignment
        {
            get { return (Corner)GetValue(ContentAlignmentProperty); }
            set { SetValue(ContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty ContentAlignmentProperty = 
            DependencyProperty.Register("ContentAlignment", typeof(Corner), typeof(Button), new PropertyMetadata(Corner.BottomLeft,
                ContentAlignmentChanged));

        private static void ContentAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = d as LabeledButton;
            Corner value = btn.ContentAlignment;
            btn.HorizontalContentAlignment = (value == Corner.TopLeft || value == Corner.BottomLeft) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            btn.VerticalContentAlignment = (value == Corner.TopLeft || value == Corner.TopRight) ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = 
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Button), 
                new PropertyMetadata(null));

        public MediaElement SoundElement
        {
            get { return (MediaElement)GetValue(SoundElementProperty); }
            set { SetValue(SoundElementProperty, value); }
        }
        public static readonly DependencyProperty SoundElementProperty = 
            DependencyProperty.Register(nameof(SoundElement), typeof(MediaElement), typeof(Button), 
                new PropertyMetadata(null));
    }
}