using System;
using System.Text;
using System.IO;

namespace escola_api.Utils
{
    public class Logger
    {
        public string Path { get; }

        public Logger(string path)
        {
            Path = path;
        }

        public void Log(Exception ex)
        {
            StringBuilder text = new StringBuilder();
            text.Append("\nData: ");
            text.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            text.Append("\nMensagem: ");
            text.Append(ex.Message);
            text.Append("\nStackTrace: ");
            text.Append(ex.StackTrace);
            text.Append("\n-------------------------------------------------------------");

            using (StreamWriter log = new StreamWriter(Path, true))
            {
                log.WriteLine(text.ToString());
            }
        }
    }
}