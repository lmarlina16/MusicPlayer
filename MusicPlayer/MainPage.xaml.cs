using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Media.Core;
using Windows.Media;
using System.Collections.ObjectModel;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Playback;


namespace MusicPlayer
{
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();
        private MediaPlaybackList mpl = new MediaPlaybackList();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder musicLib = KnownFolders.MusicLibrary;
            var files = await musicLib.GetFilesAsync();

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
        }

        #region NavigationView event handlers
        private void nvTopLevelNav_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem
            foreach (NavigationViewItemBase item in nvTopLevelNav.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Home_Page")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(MusicPlayer.Views.Song));
        }

        private void nvTopLevelNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        }

        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            if (args.IsSettingsInvoked)
            {
            //    contentFrame.Navigate(typeof(Settings));
            }
            else
            {
                TextBlock ItemContent = args.InvokedItem as TextBlock;
                if (ItemContent != null)
                {
                    switch (ItemContent.Tag)
                    {
                        case "Nav_Song":
                            contentFrame.Navigate(typeof(MusicPlayer.Views.Song));
                            break;

                        case "Nav_Album":
                            contentFrame.Navigate(typeof(MusicPlayer.Views.Album));
                            break;

                        case "Nav_Artist":
                            contentFrame.Navigate(typeof(MusicPlayer.Views.Artist));
                            break;

                        case "Nav_Playlist":
                            contentFrame.Navigate(typeof(MusicPlayer.Views.Playlist));
                            break;

                    }
                }
            }
        }
        #endregion

        private async void Mylist_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Get Selected Item
            MusicLibrary song = (MusicLibrary)e.ClickedItem;
            StorageFile file = await StorageFile.GetFileFromPathAsync(song.Path);
            //code to play song from song.MusicPath
            IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

            //set media player controls
            SetMediaPlayerControl();

            //set media player source
       //     mediaPlayer.Source = MediaSource.CreateFromStream(readStream, file.ContentType);
            mediaPlayer.AutoPlay = true;

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            this.SetMediaPlayerControl();
            mediaPlayer.Source = mpl;
            mediaPlayer.MediaPlayer.Play();

        }
        private void SetMediaPlayerControl()
        {
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
        }

    }
}
