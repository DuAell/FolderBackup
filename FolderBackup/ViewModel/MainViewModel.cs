using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FolderBackup.Extension;
using FolderBackup.Model;
using FolderBackup.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace FolderBackup.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const string ConfigurationFileName = "config.json";

        private readonly IFileSystemService _fileSystemService;
        private readonly ITimeProvider _timeProvider;

        public static readonly string BackupFolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "FolderBackup");

        public MainViewModel(IFileSystemService fileSystemService, ITimeProvider timeProvider)
        {
            _fileSystemService = fileSystemService;
            _timeProvider = timeProvider;

            ReadConfiguration();
            FindBackups();

            if (Configuration.BackupConfigurations.Count == 1)
                BackupConfigurationSelected = Configuration.BackupConfigurations.First();
        }

        #region Properties

        private Configuration _configuration;

        public Configuration Configuration
        {
            get { return _configuration; }
            set { Set(ref _configuration, value); }
        }

        private BackupConfiguration _backupConfigurationSelected;

        public BackupConfiguration BackupConfigurationSelected
        {
            get { return _backupConfigurationSelected; }
            set
            {
                if (!Set(ref _backupConfigurationSelected, value))
                    return;

                BackupSelected = value.Backups.Count == 1 ? value.Backups.First() : null;

                BackupCommand.RaiseCanExecuteChanged();
                RestoreCommand.RaiseCanExecuteChanged();
            }
        }

        private Backup _backupSelected;

        public Backup BackupSelected
        {
            get { return _backupSelected; }
            set
            {
                if (!Set(ref _backupSelected, value))
                    return;

                RestoreCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isWorking;

        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                if (!Set(ref _isWorking, value))
                    return;
                BackupCommand.RaiseCanExecuteChanged();
                RestoreCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        private void ReadConfiguration()
        {
            if (!File.Exists(ConfigurationFileName))
            {
                Configuration = new Configuration();
                return;
            }

            var configFileContent = File.ReadAllText(ConfigurationFileName);
            Configuration = JsonSerializer.Deserialize<Configuration>(configFileContent);
        }

        private void FindBackups()
        {
            DispatcherHelper.UIDispatcher.Invoke(() =>
            {
                foreach (var backupConfiguration in Configuration.BackupConfigurations)
                {
                    backupConfiguration.Backups.Clear();

                    var backupConfigurationDirectory = Path.Combine(BackupFolderPath, backupConfiguration.Name);
                    if (!Directory.Exists(backupConfigurationDirectory))
                        continue;

                    foreach (var backupDirectory in Directory.GetDirectories(backupConfigurationDirectory)
                        .Select(x => new DirectoryInfo(x)).OrderByDescending(x => x.CreationTime))
                    {
                        backupConfiguration.Backups.Add(new Backup
                        {
                            Name = backupDirectory.Name,
                            DateTime = backupDirectory.CreationTime
                        });
                    }
                }
            });
        }

        #region ICommands
        private RelayCommand _backupCommand;

        public RelayCommand BackupCommand => _backupCommand ??= new RelayCommand(async () => await BackupCommandExecuteAsync().ConfigureAwait(false), BackupRestoreCommonCanExecute);

        private RelayCommand _restoreCommand;

        public RelayCommand RestoreCommand => _restoreCommand ??= new RelayCommand(async () => await RestoreCommandExecuteAsync().ConfigureAwait(false), RestoreCommandCanExecute);

        private bool RestoreCommandCanExecute()
        {
            return BackupRestoreCommonCanExecute() && BackupSelected != null;
        }

        #endregion

        private bool BackupRestoreCommonCanExecute()
        {
            return BackupConfigurationSelected != null && !IsWorking;
        }

        private async Task BackupCommandExecuteAsync()
        {
            await CopyAsync(BackupConfigurationSelected.OriginalPath,
                Path.Combine(BackupFolderPath, BackupConfigurationSelected.Name,
                    _timeProvider.Now.ToString("yyyyMMdd_HHmmss").ToPath())).ConfigureAwait(false);

            FindBackups();
        }

        private async Task RestoreCommandExecuteAsync()
        {
            await CopyAsync(Path.Combine(BackupFolderPath, BackupConfigurationSelected.Name, BackupSelected.Name),
                BackupConfigurationSelected.OriginalPath).ConfigureAwait(false);
        }

        private async Task CopyAsync(string source, string destination)
        {
            try
            {
                IsWorking = true;
                await Task.Run(() => _fileSystemService.DirectoryCopy(source, destination, true));
                await Task.Delay(2000);
            }
            finally
            {
                IsWorking = false;
            }
        }

        public void Save()
        {
            var configJson = JsonSerializer.Serialize(Configuration);
            File.WriteAllText(ConfigurationFileName, configJson);
        }
    }
}
