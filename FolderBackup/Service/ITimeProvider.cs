using System;

namespace FolderBackup.Service
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }
}