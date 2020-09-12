using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Configuration;

namespace Business.Manager
{
    public class FileManager
    {
        public FtpWebResponse UploadPhoto(byte[] fileContents,string fileName)
        {
            string path = string.Concat(ConfigurtionOptions.FtpConnectionString, fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Credentials = new NetworkCredential(ConfigurtionOptions.FtpUser,ConfigurtionOptions.FtpPass);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.ContentLength = fileContents.Length;
            request.RenameTo = fileName;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            return (FtpWebResponse)request.GetResponse();
            //using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            //{
            //    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            //}

        }
    }
}
