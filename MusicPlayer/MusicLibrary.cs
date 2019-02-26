using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
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

        public async static Task<IReadOnlyList<StorageFile>>  GetSongFiles()
        {
            StorageFolder musicLib = KnownFolders.MusicLibrary;
            var files = await musicLib.GetFilesAsync();
            return  files; 
        }

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
        public async static Task<List<string>> GetAlbumList()
        {
            List<string> AlbumList = new List<string>();

            var files = await GetSongFiles();
            foreach (var file in files)
            {
                //get file/song property
                var musicProperties = await file.Properties.GetMusicPropertiesAsync();
                var album = musicProperties.Album;
                if (album is null || album.Equals(""))
                    album = "Unknown";

                if (!AlbumList.Contains(album))
                    AlbumList.Add(album);
            }
            AlbumList.Sort();
            return AlbumList;
        }
        public async static void GetPlaylist()
        {
            StorageFolder folder = KnownFolders.MusicLibrary;
            var myMusic = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Music);

            QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".mp4", ".m4a" });

            queryOption.FolderDepth = FolderDepth.Deep;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            var files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
              (queryOption).GetFilesAsync();

            foreach (var file in files)
            {
                // do something with the music files
                string name = file.Name;
                string type = file.ContentType;
            }
        }

     }
}
