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
        public void UploadFile(byte[] fileContents,string fileName)
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
    }
}
