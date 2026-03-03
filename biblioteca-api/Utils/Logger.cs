using System;
using System.IO;
using System.Text;

namespace biblioteca_api.Utils
{
    public class Logger
    {
        public string Path { get; }

        public Logger(string path)
        {
            Path = path;
        }

        public void Log(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nData: ");
            sb.Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.Append("\nMensagem: " + e.Message);
            sb.Append("\nStackTrace:" + e.StackTrace);
            sb.Append("\n---------------------------------------------------");

            using (StreamWriter log = new StreamWriter(Path, true))
            {
                log.WriteLine(sb.ToString());
            }
        }
    }


}