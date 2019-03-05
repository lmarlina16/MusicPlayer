using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicPlayer
{
    class MusicLibrary
    {
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Path { get; set; }
        public BitmapImage AlbumCover { get; set; }

        public async static Task<List<string>> GetArtistList()
        {
            List<string> ArtistList = new List<string>();

            var files = await GetSongFiles();
            foreach (var file in files)
            {
                //get file/song property
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var artist = musicProperties.Artist;
                if (artist is null || artist.Equals(""))
                    artist = "Unknown";
                if (!ArtistList.Contains(artist))
                    ArtistList.Add(artist);
            }
            ArtistList.Sort();
            return ArtistList;
        }
        public async static Task<IEnumerable<MusicLibrary>> GetAlbumList()
        {
            List<string> AlbumList = new List<string>();

            ObservableCollection<MusicLibrary> MusicList = new ObservableCollection<MusicLibrary>();

            var files = await GetSongFiles();
            foreach (var file in files)
            {
                StorageItemThumbnail currentThumb = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 200, ThumbnailOptions.UseCurrentScale);
                var albumCover = new BitmapImage();
                albumCover.SetSource(currentThumb);
                //get file/song property
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var album = musicProperties.Album;
                if (album is null || album.Equals(""))
                    album = "Unknown";

                if (!AlbumList.Contains(album))
                {
                    AlbumList.Add(album);
                    MusicLibrary ml = new MusicLibrary()
                    {
                        Album = album,
                        AlbumCover = albumCover
                    };
                    MusicList.Add(ml);
                }
            }

            IEnumerable<MusicLibrary> sortedAlbum = MusicList.OrderBy(o => o.Album);
            return sortedAlbum;
        }
        public async static Task<IReadOnlyList<StorageFile>> GetSongFiles()
        {
            StorageFolder folder = KnownFolders.MusicLibrary;
            var myMusic = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Music);

            QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".mp4", ".m4a" }); //".m4p" not supported: itune music file

            queryOption.FolderDepth = FolderDepth.Deep;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            var files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
              (queryOption).GetFilesAsync();

            return files;
        }

    }
}
