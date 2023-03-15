using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SSInventory.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SSInventory.Web.Services
{
    /// <summary>
    /// Clean up file service
    /// Use to delete files in the background with settings <see cref="CleanupAppSettings"/>
    /// </summary>
    public class CleanUpFileService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private static System.Timers.Timer timerWorker;
        private static CleanupAppSettings _settings;

        /// <summary>
        /// Clean up file service constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        public CleanUpFileService(IServiceProvider serviceProvider, IOptions<CleanupAppSettings> options)
        {
            _serviceProvider = serviceProvider;
            _settings = options.Value;
        }

        /// <summary>
        /// Start clean up service timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                return Task.Run(() =>
                {
                    if (timerWorker == null)
                    {
                        SetTimer();
                    }
                });
            }
        }

        /// <summary>
        /// Stop timer
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                timerWorker = null;
            });

        }

        private void SetTimer()
        {
            try
            {
                // Get timer config in appsetting.json
                timerWorker = new System.Timers.Timer(_settings.CleanupOldFilesExpiration);
                timerWorker.Elapsed += OnTimedEvent;
                timerWorker.AutoReset = false;
                timerWorker.Enabled = true;
            }
            catch
            {
                if (timerWorker != null)
                {
                    timerWorker.Dispose();
                }
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            DeleteFiles(_settings.SaveFileRootPath);
        }

        private void DeleteFiles(string directoryPath)
        {
            if (!string.IsNullOrEmpty(directoryPath))
            {
                var subFolders = Directory.GetDirectories(directoryPath, _settings.TemporaryFolderName, searchOption: SearchOption.AllDirectories);
                if (subFolders.Length > 0)
                {
                    foreach (var folder in subFolders)
                    {
                        DeleteFilesInfolder(folder);
                    }
                }

                var originFolder = Directory.GetDirectories(directoryPath, _settings.UploadOriginFolder, searchOption: SearchOption.AllDirectories);
                if(originFolder.Length > 0)
                {
                    foreach (var folder in originFolder)
                    {
                        DeleteFilesInfolder(folder);
                    }
                }
            }
        }

        private void DeleteFilesInfolder(string folderPath)
        {
            var now = DateTime.Now;
            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
                                 .Where(f => (now - File.GetCreationTime(f)).TotalDays >= _settings.DeleteFilesAfterXday);
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
