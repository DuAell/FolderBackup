using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using GalaSoft.MvvmLight;

namespace FolderBackup.Model
{
    public class BackupConfiguration : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _originalPath;

        public string OriginalPath
        {
            get { return _originalPath; }
            set { Set(ref _originalPath, value); }
        }

        private ObservableCollection<Backup> _backups = new ObservableCollection<Backup>();

        [JsonIgnore]
        public ObservableCollection<Backup> Backups
        {
            get { return _backups; }
            set { Set(ref _backups, value); }
        }
    }
}
