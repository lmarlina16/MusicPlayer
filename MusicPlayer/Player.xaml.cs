using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Media.Core;


namespace MusicPlayer
{
    public sealed partial class Player : Page
    {
        public Player()
        {
            this.InitializeComponent();
        }

        private async void LoadMediaFile(object sender, TappedRoutedEventArgs e)
        {

            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".m4a");

            StorageFile file = await picker.PickSingleFileAsync(); ;

            if (file != null)
            {
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

                mediaPlayer.AreTransportControlsEnabled = true;

                mediaPlayer.TransportControls.IsFastForwardButtonVisible = true;
                mediaPlayer.TransportControls.IsFastForwardEnabled = true;

                mediaPlayer.TransportControls.IsFastRewindButtonVisible = true;
                mediaPlayer.TransportControls.IsFastRewindEnabled = true;

                mediaPlayer.TransportControls.IsNextTrackButtonVisible = true;
                mediaPlayer.TransportControls.IsPreviousTrackButtonVisible = true;

                mediaPlayer.TransportControls.IsPlaybackRateButtonVisible = true;
                mediaPlayer.TransportControls.IsPlaybackRateEnabled = true;

                mediaPlayer.TransportControls.IsSkipForwardButtonVisible = true;
                mediaPlayer.TransportControls.IsSkipForwardEnabled = true;

                mediaPlayer.TransportControls.IsSkipBackwardButtonVisible = true;
                mediaPlayer.TransportControls.IsSkipBackwardEnabled = true;

                mediaPlayer.TransportControls.IsStopButtonVisible = true;
                mediaPlayer.TransportControls.IsStopEnabled = true;

                mediaPlayer.TransportControls.IsRightTapEnabled = true;

                mediaPlayer.SetSource(readStream, file.ContentType);
                mediaPlayer.Play();
            }

        }
    }
}
