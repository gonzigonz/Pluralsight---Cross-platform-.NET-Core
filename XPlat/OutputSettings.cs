using System.IO;

namespace XPlat
{
    public class OutputSettings
    {
        private string _path;
        private string _directory;

        public OutputSettings()
        {
            Folder = "reports";
            File = "report.txt";
        }
        public string Folder { get; set; }
        public string File { get; set; }

        public string ReportFilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_path))
                {
                    _path = Path.Combine(Directory.GetCurrentDirectory(), Folder, File);
                }
                return _path;
            }
        }

        public string ReportDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_directory))
                {
                    _directory = Path.GetDirectoryName(ReportFilePath);
                }
                return _directory;
            }
        }
    }
}
