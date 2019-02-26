﻿using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Media.Core;
using System.Collections.ObjectModel;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace MusicPlayer.Views
{
    public sealed partial class Song : Page
    {
        private MediaPlaybackList mpl = new MediaPlaybackList();

        public Song()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder musicLib = KnownFolders.MusicLibrary;
            var files = await musicLib.GetFilesAsync();

            ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();

            foreach (var file in files)
            {
                //get file/song property
                StorageItemThumbnail currentThumb = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 200, ThumbnailOptions.UseCurrentScale);
                var albumCover = new BitmapImage();
                albumCover.SetSource(currentThumb);
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var title = musicProperties.Title;
                var artist = musicProperties.Artist;
                var album = musicProperties.Album;

                //build songs list
                MusicList.Add(new MusicLibrary
                {
                    Filename = file.Name,
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Path = file.Path
                });

                MediaPlaybackItem item = new MediaPlaybackItem(MediaSource.CreateFromStorageFile(file));
                mpl.Items.Add(item);
            }
            myList.ItemsSource = MusicList;
 //           SetMediaPlayerControl();
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = mpl;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = false;
           
        }
        private async void Mylist_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Get Selected Item
            MusicLibrary song = (MusicLibrary)e.ClickedItem;
            StorageFile file = await StorageFile.GetFileFromPathAsync(song.Path);
            //code to play song from song.MusicPath
            IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

            //set media player controls
           // SetMediaPlayerControl();

            //set media player source
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = MediaSource.CreateFromStream(readStream, file.ContentType);
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = true;
        }
        private void SetMediaPlayerControl()
        {
            //mediaPlayer.AreTransportControlsEnabled = true;
            
            //mediaPlayer.TransportControls.IsFastForwardButtonVisible = true;
            //mediaPlayer.TransportControls.IsFastForwardEnabled = true;

            //mediaPlayer.TransportControls.IsFastRewindButtonVisible = true;
            //mediaPlayer.TransportControls.IsFastRewindEnabled = true;

            //mediaPlayer.TransportControls.IsNextTrackButtonVisible = true;
            //mediaPlayer.TransportControls.IsPreviousTrackButtonVisible = true;

            //mediaPlayer.TransportControls.IsPlaybackRateButtonVisible = true;
            //mediaPlayer.TransportControls.IsPlaybackRateEnabled = true;

            //mediaPlayer.TransportControls.IsSkipForwardButtonVisible = true;
            //mediaPlayer.TransportControls.IsSkipForwardEnabled = true;

            //mediaPlayer.TransportControls.IsSkipBackwardButtonVisible = true;
            //mediaPlayer.TransportControls.IsSkipBackwardEnabled = true;

            //mediaPlayer.TransportControls.IsStopButtonVisible = true;
            //mediaPlayer.TransportControls.IsStopEnabled = true;

            //mediaPlayer.TransportControls.IsRightTapEnabled = true;
        }



    }
}
