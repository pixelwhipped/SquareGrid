using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SquareGrid.Utilities
{
    public static class AsyncIO
    {
        public static bool DoesFileExistAsync(StorageFolder folder, string fileName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var f = await folder.GetFileAsync(fileName);
                    return f != null;
                }
                catch
                {
                    return false;
                }
            }).Result;
        }

        public static bool DoesFolderExistAsync(StorageFolder folder, string folderName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var f = await folder.GetFolderAsync(folderName);
                    return f != null;
                }
                catch
                {
                    return false;
                }
            }).Result;
        }

        public static IList<StorageFile> GetFilesAsync(StorageFolder folder)
        {
            var files = Task.Run(async () =>
            {
                try
                {
                    return await folder.GetFilesAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
            }).Result;
            var ret = new List<StorageFile>();
            if (files != null) ret.AddRange(files);
            return ret;
        }

        public static StorageFolder CreateFolderAsync(StorageFolder folder, string folderName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    return await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                }
                catch
                {
                    return null;
                }
            }).Result;
        }

        public static StorageFile CreateFileAsync(StorageFolder folder, string fileName)
        {
            return Task.Run(async () =>
            {
                try
                {
                    return await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                }
                catch
                {
                    return null;
                }
            }).Result;
        }


        public static string ReadTextFileAsync(StorageFile file)
        {
            return Task.Run(async () => await FileIO.ReadTextAsync(file)).Result;
        }
    }
}
