using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        internal override void UpdateCorners()
        {
            double radius = this.ActualHeight / 2;
            CornerRadius roundRight = new CornerRadius(0, radius, radius, 0);
            CornerRadius roundLeft = new CornerRadius(radius, 0, 0, radius);
            CornerRadius notRound = new CornerRadius(0);
             
            Thickness padLeft = new Thickness(radius,0,5,0);
            Thickness padRight = new Thickness(5,0,radius,0);
            Thickness noPad = new Thickness(5,0,5,0);
            
            switch (Stumps)
            {
                case Stumps.None:
                    this.CornerRadius = new CornerRadius(0);
                    this.Padding = new Thickness(5, 0, 5, 0);
                    (GetTemplateChild("rect") as Border).CornerRadius = new CornerRadius(0);
                    break;
                case Stumps.Left:
                    (GetTemplateChild("rect") as Border).CornerRadius = (Direction == FlowDirection.LeftToRight) ? roundLeft : notRound;
                    this.CornerRadius = (Direction == FlowDirection.LeftToRight) ? notRound : roundLeft;
                    this.Padding = (Direction == FlowDirection.RightToLeft) ? padLeft : noPad;
                    break;
                case Stumps.Right:
                    (GetTemplateChild("rect") as Border).CornerRadius = (Direction == FlowDirection.RightToLeft) ? roundRight : notRound;
                    this.CornerRadius = (Direction == FlowDirection.RightToLeft) ? notRound : roundRight;
                    this.Padding = (Direction == FlowDirection.LeftToRight) ? padRight : noPad;
                    break;
                case Stumps.Both:
                    (GetTemplateChild("rect") as Border).CornerRadius = (Direction == FlowDirection.LeftToRight) ? roundLeft : roundRight;
                    this.CornerRadius = (Direction == FlowDirection.LeftToRight) ? roundRight : roundLeft;
                    this.Padding = (Direction == FlowDirection.LeftToRight) ? padRight : padLeft;
                    break;
            }
        }

        internal override void SetMasks(bool isLit)
        {
            try
            {
                (this.GetTemplateChild("mask2") as Rectangle).Fill = (isLit) ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Color.FromArgb(0x7F, 0x00, 0x00, 0x00));
                (this.GetTemplateChild("mask") as Border).Background = (isLit) ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Color.FromArgb(0x7F, 0x00, 0x00, 0x00));
            }
            catch
            { }            
        }

    }
}
