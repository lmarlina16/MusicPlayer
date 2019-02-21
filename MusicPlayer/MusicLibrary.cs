using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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

        public BitmapImage AlbumCover { get; set; }

        /*      public MusicLibrary GetList = new MusicLibrary
              {
                  Filename = "",
                  Title = "",
                  Artist = "",
                  Album = ""
              };
      */
        public async static void GetPlaylist()
        {
            StorageFolder folder = KnownFolders.MusicLibrary;
            var myMusic = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Music);

            GetSongs();
        }

        public async static void GetSongs()
        {
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
