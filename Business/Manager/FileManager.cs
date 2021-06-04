using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Configuration;
using Data.Repositories;
using PdfSharp.Drawing;
using static Business.Models.Dictionary.StandartDictionaries;

namespace Business.Manager
{
    public class FileManager
    {
        private readonly TaskRepository _taskRepository;

        public FileManager(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public string UploadFile(byte[] fileContents,string fileName)
        {
            try
            {
                if (!DirectoryExists(ConfigurtionOptions.FtpFolder))
                {
                    MakeDirectory(ConfigurtionOptions.FtpFolder);
                }

                string filePath = string.Concat(ConfigurtionOptions.FtpFolder, fileName);
                string path = string.Concat(ConfigurtionOptions.FtpConnectionString, filePath);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(path));
                request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser, ConfigurtionOptions.FtpPass);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Flush();
                }

                return path;
            }
            catch (WebException e)
            {
                throw new Exception(((FtpWebResponse)e.Response).StatusDescription);
            }
        }

        public bool MakeDirectory(string folder)
        {
            string path = string.Concat(ConfigurtionOptions.FtpConnectionString, folder);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(path));
            request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser, ConfigurtionOptions.FtpPass);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            return request.GetResponse() != null;
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                string path = string.Concat(ConfigurtionOptions.FtpConnectionString, fileName);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(path));
                request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser, ConfigurtionOptions.FtpPass);
                request.Method = WebRequestMethods.Ftp.DeleteFile;

                return request.GetResponse() != null;
            }
            catch (WebException e)
            {
                throw new Exception("DeleteFile:"+ fileName + ((FtpWebResponse)e.Response).StatusDescription);
            }
        }

        public bool DirectoryExists(string folder)
        {
            try
            {
                string path = string.Concat(ConfigurtionOptions.FtpConnectionString, folder);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(path));
                request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser, ConfigurtionOptions.FtpPass);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                return request.GetResponse() != null;
            }
            catch
            {
                return false;
            }
        }

        //---------------------------------------
        /// <summary>
        /// Объединенные файлы в один файл
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string GetMergedFilePath(int taskId)
        {
            // -------------------------------------------

            // -------------------------------------------


            IEnumerable<Bitmap> images = GetImagesByTask(taskId);
            var enumerable = images as IList<Bitmap> ?? images.ToList();

            var width = 0;
            var height = 0;

            foreach (var image in enumerable)
            {
                width += image.Width;
                height = image.Height > height
                    ? image.Height
                    : height;
            }

            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                var localWidth = 0;
                foreach (var image in enumerable)
                {
                    g.DrawImage(image, localWidth, 0);
                    localWidth += image.Width;
                }
            }
            return UploadFile(ImageToByteArray(bitmap), "splitted_file" + DateTime.Now.ToString() );
        }

        /// <summary>
        /// Возвращает Фото акта
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        private List<Bitmap> GetImagesByTask(int taskId)
        {
            var lstbitmap = new List<Bitmap>();

            var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Act_Photo");
            var files = _taskRepository.FilesByTask(new Data.Queries.Task.TaskQuery { TaskId = taskId });
            foreach (var file in files.Where(x=>x.TypeId == hFileType.Id ) )
            {
                var filePath = file.Path;
                var request = WebRequest.Create(filePath);
                request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser, ConfigurtionOptions.FtpPass);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var img = Image.FromStream(stream))
                {
                    var bitmap = (Bitmap)img;
                    lstbitmap.Add(bitmap);
                }
            }
            return lstbitmap;
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }


        public string CreatePhotosPDF(int taskId)
        {
            var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Act_Photo");
            var files = _taskRepository.FilesByTask(new Data.Queries.Task.TaskQuery { TaskId = taskId });
            if (files.Count <= 0)
            {
                return "";
            }


            dynamic fileName = taskId.ToString() + "_" + DateTime.Now.Ticks + ".pdf";
            var pdfGeneratePath = ConfigurtionOptions.FtpFolder + "/pdf";
            if (!DirectoryExists(pdfGeneratePath))
            {
                MakeDirectory(pdfGeneratePath);
            }

            string filePathPdf = string.Concat(pdfGeneratePath, fileName);
            string dest = string.Concat(ConfigurtionOptions.FtpConnectionString, filePathPdf);

            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
            // --------------------------------------------

            foreach (var file in files.Where(x => x.TypeId == hFileType.Id))
            {
                var filePath = file.Path;
                var request = WebRequest.Create(filePath);
                request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser, ConfigurtionOptions.FtpPass);
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var imgSource = Image.FromStream(stream))
                {
                    MemoryStream strm = new MemoryStream();
                    imgSource.Save(strm, System.Drawing.Imaging.ImageFormat.Jpeg);

                    using (XImage img = XImage.FromStream(strm))
                    {
                        img.Interpolate = false;
                        int width = img.PixelWidth;
                        int height = img.PixelHeight;
                        PdfSharp.Pdf.PdfPage page = new PdfSharp.Pdf.PdfPage
                        {
                            Width = width,
                            Height = height
                        };
                        doc.Pages.Add(page);
                    }                    
                }
            }
            // --------------------------------------------
            doc.Save(dest);
            doc.Dispose();

            return dest;
        }

    }
}
