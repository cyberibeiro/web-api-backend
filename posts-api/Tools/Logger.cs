using System;
using System.Text;
using System.IO;

namespace posts_api.Tools
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
            StringBuilder exception = new StringBuilder();
            exception.Append("\nData: ");
            exception.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            exception.Append("\nMensagem: ");
            exception.Append(e.Message);
            exception.Append("\nStackTrace: ");
            exception.Append(e.StackTrace);
            exception.Append("\n-----------------------------------------------------------------");

            using (StreamWriter log = new StreamWriter(Path, true))
            {
                log.WriteLine(exception.ToString());
            }
        }
    }
}