using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
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
    public sealed partial class Settings
    {
        // The guidelines recommend using 100px offset for the content animation.
        const int ContentAnimationOffset = 100;
        public BaseGame ParentInterface;

        public Settings(BaseGame parent)
        {
            ParentInterface = parent;
            InitializeComponent();

            AudioState.IsOn = ParentInterface.Audio.EnableAudio;

            EffectVolume.Value = ParentInterface.Audio.EffectVolume*100f;
            MusicVolume.Value = ParentInterface.Audio.MusicVolume*100f;

            SettingsContent.Transitions = new TransitionCollection
            {
                new EntranceThemeTransition
                {
                    FromHorizontalOffset =
                        (SettingsPane.Edge == SettingsEdgeLocation.Right)
                            ? ContentAnimationOffset
                            : (ContentAnimationOffset*-1)
                }
            };
            Header.DataContext = Branding.Current;
            Panel.DataContext = Branding.Current;
            Branding.Current.PropertyChanged += (o, e) => { Header.Background = Branding.Current.ForegroundBrush; };
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            // First close our Flyout.
            var parent = Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }

            // If the app is not snapped, then the back button shows the Settings pane again.
            if (Windows.UI.ViewManagement.ApplicationView.Value != Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                SettingsPane.Show();
            }
        }

        private void AudioState_Toggled(object sender, RoutedEventArgs e)
        {
            ParentInterface.Audio.EnableAudio = AudioState.IsOn;
        }

        private void EffectVolume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ParentInterface.Audio.EffectVolume = (float)EffectVolume.Value/100f;
        }

        private void MusicVolume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ParentInterface.Audio.MusicVolume = (float)MusicVolume.Value/100f;
        }
    }
}
