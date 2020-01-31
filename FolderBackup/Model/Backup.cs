using System;
using GalaSoft.MvvmLight;

namespace FolderBackup.Model
{
    public class Backup : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private DateTime _dateTime;

        public DateTime DateTime
        {
            get { return _dateTime; }
            set { Set(ref _dateTime, value); }
        }
    }
}
