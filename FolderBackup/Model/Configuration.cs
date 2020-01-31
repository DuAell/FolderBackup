using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace FolderBackup.Model
{
    public class Configuration : ObservableObject
    {
        private ObservableCollection<BackupConfiguration> _backupConfigurations = new ObservableCollection<BackupConfiguration>();

        public ObservableCollection<BackupConfiguration> BackupConfigurations
        {
            get { return _backupConfigurations; }
            set { Set(ref _backupConfigurations, value); }
        }
    }
}
