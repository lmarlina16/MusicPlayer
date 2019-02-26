using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicPlayer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Album : Page
    {

        private List<string> AlbumList = new List<string>();
        private List<string> songsByAlbumList = new List<string>();
        private MediaPlaybackList mpl = new MediaPlaybackList();
        private ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();

        public Album()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var files = await MusicLibrary.GetSongFiles();
            AlbumList = await MusicLibrary.GetAlbumList();
            lvAlbums.ItemsSource = AlbumList;
        }

        private async void LvSongsByAlbum_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Get Selected Item
            MusicLibrary song = (MusicLibrary)e.ClickedItem;
            StorageFile file = await StorageFile.GetFileFromPathAsync(song.Path);
            //code to play song from song.MusicPath
            IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

            //set media player source
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = MediaSource.CreateFromStream(readStream, file.ContentType);
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = true;

        }

        private async void LvAlbums_ItemClick(object sender, ItemClickEventArgs e)
        {
            var album = (string)e.ClickedItem;

            StorageFolder musicLib = KnownFolders.MusicLibrary;
            var files = await musicLib.GetFilesAsync();

            ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();

            foreach (var file in files)
            {
                //get file/song property
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var title = musicProperties.Title;
                var artist = musicProperties.Artist;

                if (musicProperties.Album == album)
                {
                    MediaPlaybackItem item = new MediaPlaybackItem(MediaSource.CreateFromStorageFile(file));
                    mpl.Items.Add(item);

                    //build songs list
                    MusicList.Add(new MusicLibrary
                    {
                        Filename = file.Name,
                        Title = title,
                        Artist = artist,
                        Album = album,
                        Path = file.Path
                    });
                }
            }
            lvSongsByAlbum.ItemsSource = MusicList;
            lvSongsByAlbum.Visibility = Visibility.Visible;
            lvAlbums.Visibility = Visibility.Collapsed;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = mpl;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = false;

        }
    }
}
