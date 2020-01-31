using System;

namespace FolderBackup.Service.Implementation
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
