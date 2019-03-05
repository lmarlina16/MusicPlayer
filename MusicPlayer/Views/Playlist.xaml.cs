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
using Windows.Storage.Pickers;
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
    public sealed partial class Playlist : Page
    {
        public Playlist()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".m4a");

            IReadOnlyList<StorageFile> files = await picker.PickMultipleFilesAsync(); ;
            MediaPlaybackList mpl = new MediaPlaybackList();
            ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();

            foreach (var file in files)
            {
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                //get file/song property
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var title = musicProperties.Title;
                var album = musicProperties.Album;
                var artist = musicProperties.Artist;
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
            lvPlaylist.ItemsSource = MusicList;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = mpl;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = true;
        }

        private async void LvPlaylist_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Get Selected Item
            MusicLibrary song = (MusicLibrary)e.ClickedItem;
            StorageFile file = await StorageFile.GetFileFromPathAsync(song.Path);
            //code to play song from song.MusicPath
            IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);

            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.Source = MediaSource.CreateFromStream(readStream, file.ContentType); ;
            ((Window.Current.Content as Frame).Content as MainPage).mediaPlayer.AutoPlay = true;

        }
    }
}
