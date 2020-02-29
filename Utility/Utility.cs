using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class Utility
    {
        public static string MasterDBConnectionstring
        {
            get { return ConfigurationManager.ConnectionStrings["dbMasterConnectionstring"].ConnectionString; }
        }

        public static string GetTransactionDBConnectionstring(string DbServerName, string DbName)
        {
            string Connectionstring = null;
            try
            {
                Connectionstring = string.Format(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString, DbServerName, DbName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Connectionstring;
        }

        public static void WriteExceptionLog(Exception ex)
        {
            try
            {
                string directoryLocation = Utility.GetConfigParam<string>("PhysicalDocumentPath");
                string fileToWrite = directoryLocation + Utility.GetConfigParam<string>("HiveAuditErrorFile");
                string userLogFileName = string.Empty;
                if (null != fileToWrite && fileToWrite != "")
                {

                    if (!System.IO.Directory.Exists(directoryLocation))
                        System.IO.Directory.CreateDirectory(directoryLocation);

                    using (StreamWriter sw = new StreamWriter(fileToWrite, true))
                    {
                        sw.Write("================================\n\r\n");
                        sw.Write("\nDate ---\n{0}", DateTime.Now + "\n\r\n");
                        sw.Write("\nMessage ---\n{0}", ex.Message + "\n\r\n");
                        sw.Write("\nHelpLink ---\n{0}", ex.HelpLink + "\n\r\n");
                        sw.Write("\nSource ---\n{0}", ex.Source + "\n\r\n");
                        sw.Write(
                            "\nStackTrace ---\n{0}", ex.StackTrace + "\n\r\n");
                        sw.Write(
                            "\nTargetSite ---\n{0}", ex.TargetSite + "\n\r\n");
                        sw.Close();
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static void WritemessageToFile(string str)
        {
            try
            {
                string directoryLocation = Utility.GetConfigParam<string>("PhysicalDocumentPath");
                string fileToWrite = directoryLocation + Utility.GetConfigParam<string>("HiveAuditErrorFile");
                string userLogFileName = string.Empty;
                if (null != fileToWrite && fileToWrite != "")
                {
                    Console.WriteLine(str);
                    if (!System.IO.Directory.Exists(directoryLocation))
                        System.IO.Directory.CreateDirectory(directoryLocation);

                    using (StreamWriter sw = new StreamWriter(fileToWrite, true))
                    {
                        sw.Write("================================\n\r\n");
                        sw.Write("\nDate ---\n{0}", DateTime.Now + "\n\r\n");
                        sw.Write(str + "\n\r\n");
                        sw.Close();
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static void WriteQuery(string str, int loop, int org, int record)
        {
            try
            {
                string directoryLocation = Utility.GetConfigParam<string>("PhysicalDocumentPath");
                string fileToWrite = directoryLocation + "SparkQuery_" + loop.ToString() + ".log";
                string userLogFileName = string.Empty;
                if (null != fileToWrite && fileToWrite != "")
                {
                    if (!System.IO.Directory.Exists(directoryLocation))
                        System.IO.Directory.CreateDirectory(directoryLocation);

                    using (StreamWriter sw = new StreamWriter(fileToWrite, true))
                    {
                        sw.Write("================================\n\r\n");
                        sw.Write("\nDate ---\n{0}", DateTime.Now + "\n\r\n");
                        sw.Write("Organization: {0}, Record: {1}", org, record + "\n\r\n");
                        sw.Write(str + "\n\r\n");
                        sw.Close();
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        public static T GetConfigParam<T>(string configParam)
        {
            T objSession = default(T);
            object configObj = null;
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings.Get(configParam) != null)
                {
                    configObj = (object)System.Configuration.ConfigurationManager.AppSettings.Get(configParam);
                    if ((System.Type)configObj.GetType() == typeof(T))
                        objSession = (T)(object)System.Configuration.ConfigurationManager.AppSettings.Get(configParam);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
            return objSession;
        }
    }
}
