using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LCARSToolkit.Controls
{
    public static class Extensions
    {
        public static DispatcherTimer FlashTimer { get; private set; } = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(0.5) };

        static Extensions()
        {
            FlashTimer.Start();
        }
    }
}
