using System;
using System.IO;

namespace FlavorFi.Console.DataLoads
{
    public class BaseDataLoad
    {
        public string UserToken { get; private set; }
        public Guid CompanySiteId { get; private set; }
        public string Log { get; set; }

        public BaseDataLoad(string userToken, Guid companySiteId)
        {
            this.UserToken = userToken;
            this.CompanySiteId = companySiteId;
        }

        public void ConsoleLog(string message, bool addDate = false)
        {
            if (addDate) message += " [" + DateTime.Now + "]";
            System.Console.WriteLine(message);
            this.Log += message + Environment.NewLine;
        }

        public void WriteFile(string logName)
        {
            var path = @"C:\Logs\" + logName + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt";
            if (!Directory.Exists(@"C:\Logs\"))
                Directory.CreateDirectory(@"C:\Logs\");
            if (!File.Exists(path))
                File.WriteAllText(path, this.Log);
        }
    }
}