using System.IO;

namespace FolderBackup.Service.Implementation
{
    public class FileSystemService : IFileSystemService
    {
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                file.CopyTo(Path.Combine(destDirName, file.Name), true);
            }

            if (!copySubDirs) return;
            // If copying subdirectories, copy them and their contents to new location.
            foreach (var subDir in dir.GetDirectories())
            {
                DirectoryCopy(subDir.FullName, Path.Combine(destDirName, subDir.Name), true);
            }
        }
    }
}
