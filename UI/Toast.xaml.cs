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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SquareGrid.UI
{
    public sealed partial class Toast
    {
        // The guidelines recommend using 100px offset for the content animation.
        const int ContentAnimationOffset = 100;
        private readonly DispatcherTimer _dispatcherTimer;

        public Toast(string title, string message, TimeSpan time)
        {
            InitializeComponent();
            ToastContent.Transitions = new TransitionCollection
            {
                new EntranceThemeTransition
                {
                    FromVerticalOffset = ContentAnimationOffset*-1
                }
            };
            ToastContent.DataContext = Branding.Current;
            Branding.Current.PropertyChanged += (o, e) => { ToastContent.Background = Branding.Current.ForegroundBrush; };
            Title.Text = title;
            Message.Text = message;
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (o, e) => Hide();
            _dispatcherTimer.Interval = time;
            _dispatcherTimer.Start();

        }

        private void Dismiss(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void Hide()
        {
            _dispatcherTimer.Stop();
            var parent = Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }
        }
    }
}
