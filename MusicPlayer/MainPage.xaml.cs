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
            mediaPlayer.AutoPlay = true;

        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Source = mpl;
            mediaPlayer.MediaPlayer.Play();
        }
    }
}
