using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NLog;
using NLog.Internal;
using ProviderService.Clients;
using ProviderService.Converters;
using ProviderService.Models;
using TestTask.Common.DAL.Models;

namespace ProviderService.Providers
{
    public class FTPProvider : IProvider
    {
        private readonly IConverter _converter;
        private readonly FtpOptions _options;
        private readonly FtpClient _ftpClient;
        private readonly Logger _log;
        public string MaxProcessedId { get; set; }

        public FTPProvider(IConverter converter, FtpOptions ftpOptions)
        {
            _converter = converter;
            _options = ftpOptions;
            _ftpClient = new FtpClient(_options.BaseAddress, _options.Login, _options.Password);
            _log = LogManager.GetCurrentClassLogger();
        }

        public MeasurementCollection GetData()
        {
            SetWorkingFolder(_options.WorkingFolder);

            var filelist = ListDirectory();
            var filteredFiles = Filter(filelist).ToArray();
            if (!filteredFiles.Any())
                return null;

            var list = new List<Measurement>();
            foreach (var file in filteredFiles)
            {
                try
                {
                    list.Add(_converter.Convert(ReadFile(file)));
                }
                catch (Exception ex)
                {
                    _log.Warn("Exception while process file {0}. exception message: {1}. File skiped.", file, ex.Message);
                }
            }
            var measurements = list.ToArray();

            return new MeasurementCollection(measurements) { MeasurementKeys = filteredFiles.Select(Path.GetFileNameWithoutExtension).ToArray() };

        }

        // get filelist
        private string[] ListDirectory()
        {
            return _ftpClient.ListDirectory();
        }

        // navigate to working folder
        private void SetWorkingFolder(string folderName)
        {
            _ftpClient.ChangeWorkingDirectory(folderName);
        }

        private byte[] ReadFile(string path)
        {
            var tPath = _options.TempFolder;
            if (!Directory.Exists(tPath))
                Directory.CreateDirectory(tPath);
            tPath = Path.Combine(@"C:\temp\", path);
            var status = _ftpClient.DownloadFile(path, tPath);

            return File.ReadAllBytes(tPath);
        }

        // get only new files
        private IEnumerable<string> Filter(string[] files)
        {
            // if no id key 
            if (string.IsNullOrEmpty(MaxProcessedId))
                return files;

            int lastIndex;
            if (!Int32.TryParse(MaxProcessedId, out lastIndex))
                // if id key !int
                return files;

            Func<string, bool> newFile = (s) =>
            {
                int tInt;
                return Int32.TryParse(Path.GetFileNameWithoutExtension(s), out tInt) && tInt > lastIndex;
            };

            return files.Where(newFile).ToArray();
        }
    }

    public class FtpOptions
    {
        public string BaseAddress { get; set; }
        public string WorkingFolder { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string TempFolder { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("BaseAddress: " + BaseAddress);
            sb.AppendLine("WorkingFolder: " + WorkingFolder);
            sb.AppendLine("Login: " + Login);
            sb.AppendLine("TempFolder: " + TempFolder);
            return sb.ToString();
        }
    }

}
