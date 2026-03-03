using System;
using System.Text;
using System.IO;


namespace web_api.Tools
{
    public class Logger
    {
        public string Path { get; }

        public Logger(string path)
        {
            Path = path;
        }

        public void log(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nData: ");
            sb.Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.Append("\nMessagem: ");
            sb.Append(e.Message);
            sb.Append("\nStackTrace: ");
            sb.Append(e.StackTrace);
            sb.Append("\n------------------------------------------------");

            using (StreamWriter log = new StreamWriter(Path, true))
            {
                log.WriteLine(sb.ToString());
            }
        }
    }
}