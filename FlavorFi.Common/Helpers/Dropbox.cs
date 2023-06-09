using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlavorFi.Common.Helpers
{
    public class Dropbox
    {
        public DropboxClient Client { get; private set; }
        public string FolderPath { get; private set; }

        /// <summary>
        /// Declares a new dropnet client.
        /// </summary>
        /// <param name="_folderPath">The folder path that will be used.</param>
        public Dropbox(string _folderPath)
        {
            try
            {
                this.Client = new DropboxClient("XxpSDQxAPFAAAAAAAAAvguYNb-hzlzMVVeudAZe9zBNmxObxLY6GkO5t0VdXQlJx");
                this.FolderPath = _folderPath.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes all files in the specified folder
        /// </summary>
        /// <returns>A bool value if all files were successfully deleted.</returns>
        public bool DeleteAllFilesInFolder()
        {
            try
            {
                var filesResult = this.Client.Files.ListFolderAsync(this.FolderPath).Result;
                foreach (var file in filesResult.Entries)
                {
                    var result = this.Client.Files.DeleteV2Async(this.FolderPath + file.Name).Result;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Uploads a new file to the specified path
        /// </summary>
        /// <param name="fileName">The file name to be uploaded.</param>
        /// <param name="fileData">The file byte array to be uploaded.</param>
        /// <returns>A bool value if the file was successfully uploaded.</returns>
        public bool UploadFile(string fileName, byte[] fileData)
        {
            try
            {
                using (var stream = new MemoryStream(fileData))
                {
                    var result = this.Client.Files.UploadAsync(this.FolderPath + fileName, WriteMode.Overwrite.Instance, body: stream).Result;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Downloads a file from dropbox
        /// </summary>
        /// <param name="fileName">The file name to download</param>
        /// <returns>byte[]</returns>
        public byte[] DownloadFile(string fileName)
        {
            try
            {
                var result = this.Client.Files.DownloadAsync(this.FolderPath + fileName).Result;
                var fileArr = result.GetContentAsByteArrayAsync().Result;
                if (fileArr == null) throw new ApplicationException("File does not exist.");
                else return fileArr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the file in dropbox
        /// </summary>
        /// <param name="fileName">The file name to update</param>
        /// <param name="createFile">Whether or not to create the file if it does not exists.</param>
        /// <returns>bool</returns>
        public bool UpdateFile(string fileName, byte[] fileData, bool createFile)
        {
            try
            {
                var file = this.GetAllFilesInFolder().FirstOrDefault(f => f.Item1.Equals(fileName, StringComparison.CurrentCultureIgnoreCase));
                if (file == null)
                {
                    if (!createFile) throw new ApplicationException("File does not exist.");
                    using (var stream = new MemoryStream(fileData))
                    {
                        var result = this.Client.Files.UploadAsync(this.FolderPath + fileName, WriteMode.Overwrite.Instance, body: stream).Result;
                        return true;
                    }
                }
                else
                {
                    var downloadResult = this.Client.Files.DownloadAsync(this.FolderPath + fileName).Result;
                    var currentArr = downloadResult.GetContentAsByteArrayAsync().Result;
                    var newArr = currentArr.Concat(fileData).ToArray();
                    using (var stream = new MemoryStream(newArr))
                    {
                        var result = this.Client.Files.UploadAsync(this.FolderPath + fileName, WriteMode.Overwrite.Instance, body: stream).Result;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all files in the specified folder
        /// </summary>
        public List<Tuple<string, byte[]>> GetAllFilesInFolder()
        {
            try
            {
                var lst = new List<Tuple<string, byte[]>>();
                var filesResult = this.Client.Files.ListFolderAsync(this.FolderPath).Result;
                foreach (var file in filesResult.Entries.Where(f => f.IsFile))
                {
                    var result = this.Client.Files.DownloadAsync(this.FolderPath + file.Name).Result;
                    lst.Add(new Tuple<string, byte[]>(file.Name, result.GetContentAsByteArrayAsync().Result));
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Moves a file to a new folder location
        /// </summary>
        /// <returns>A bool value if all files were successfully moved.</returns>
        public void MoveFileToNewFolder(string fileName, string newPath)
        {
            try
            {
                var from = this.FolderPath + fileName;
                var to = this.FolderPath + newPath + fileName;
                var result = this.Client.Files.MoveV2Async(from, to, autorename: true).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}