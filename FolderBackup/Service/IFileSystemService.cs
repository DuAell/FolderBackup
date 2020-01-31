namespace FolderBackup.Service
{
    public interface IFileSystemService
    {
        void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs);
    }
}