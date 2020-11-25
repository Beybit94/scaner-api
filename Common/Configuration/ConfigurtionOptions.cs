using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configuration
{
    public class ConfigurtionOptions
    {
        #region Main ConnectionString

        private static string _mainConnectionString;
        public static string MainConnectionString
        {
            get
            {
                return _mainConnectionString ?? (_mainConnectionString = ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString);
            }
        }

        #endregion

        #region WebProject ConnectionString

        private static string _webProjectConnectionString;
        public static string WebProjectConnectionString
        {
            get
            {
                return _webProjectConnectionString ?? (_webProjectConnectionString = ConfigurationManager.ConnectionStrings["WebProjectConnectionString"].ConnectionString);
            }
        }

        #endregion

        #region FtpConnectionString

        private static string _ftpConnectionString;
        public static string FtpConnectionString
        {
            get
            {
                return _ftpConnectionString ?? (_ftpConnectionString = ConfigurationManager.AppSettings["FtpConnection"]);
            }
        }

        private static string _ftpFolder;
        public static string FtpFolder
        {
            get
            {
                return _ftpFolder ?? (_ftpFolder = ConfigurationManager.AppSettings["FtpFolder"]);
            }
        }

        private static string _ftpUser;
        public static string FtpUser
        {
            get
            {
                return _ftpUser ?? (_ftpUser = ConfigurationManager.AppSettings["FtpUser"]);
            }
        }

        private static string _ftpPass;
        public static string FtpPass
        {
            get
            {
                return _ftpPass ?? (_ftpPass = ConfigurationManager.AppSettings["FtpPass"]);
            }
        }

        #endregion
    }
}
