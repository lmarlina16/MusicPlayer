using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.Storage.Streams;
using System.Collections.ObjectModel;
using MusicPlayer;

namespace MusicPlayer.Views
{
    public sealed partial class Artist : Page
    {
        private List<string> ArtistList = new List<string>();
        private List<string> songsByArtistList = new List<string>();
        private MediaPlaybackList mpl = new MediaPlaybackList();

        public Artist()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var files = await MusicLibrary.GetSongFiles();
            ArtistList = await MusicLibrary.GetArtistList();
            lvArtists.ItemsSource = ArtistList;
        }

        private async void LvArtists_ItemClick(object sender, ItemClickEventArgs e)
        {
            var artist = (string)e.ClickedItem;

            StorageFolder musicLib = KnownFolders.MusicLibrary;
            var files = await musicLib.GetFilesAsync();

            ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();
    
            foreach (var file in files)
            {
                //get file/song property
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var title = musicProperties.Title;
                var album = musicProperties.Album;

                if (musicProperties.Artist == artist)
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
            lvSongsByArtist.ItemsSource = MusicList;
            lvSongsByArtist.Visibility = Visibility.Visible;
            lvArtists.Visibility = Visibility.Collapsed;

            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = mpl;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = false;

        }

        private async void LvSongsByArtist_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Get Selected Item
            MusicLibrary song = (MusicLibrary)e.ClickedItem;
            StorageFile file = await StorageFile.GetFileFromPathAsync(song.Path);
            //code to play song from song.MusicPath
            IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

            //set media player source
            ((Window.Current.Content as Frame).Content as MainPage). mediaPlayer.Source = MediaSource.CreateFromStream(readStream, file.ContentType);
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = true;



        }
    }
}
