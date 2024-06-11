using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;

namespace WebAppForm
{
    public class EventLogging
    {
        /// <summary>
        /// This function logs messages in a text file incl. exceptions and status info.
        /// </summary>
        /// <param name="text"></param>
        public static void LogEvents(string text)
        {
            string logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                if (!Directory.Exists(logFilePath))
                    Directory.CreateDirectory(logFilePath);
                StreamWriter sw = new StreamWriter(logFilePath + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt", true);
                //Write a line of text
                sw.WriteLine(DateTime.Now.ToShortTimeString() + ": " + text);
                //Close the file
                sw.Close();
            }
            catch
            {
            }
        }

        public string GetIP()
        {
            string stringIpAddress;
            HttpRequest currentRequest = HttpContext.Current.Request;
            stringIpAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
            {
                stringIpAddress = currentRequest.ServerVariables["REMOTE_ADDR"];//we can use REMOTE_ADDR
            }
            return stringIpAddress;

        }
    }
}
