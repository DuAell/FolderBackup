using System.IO;

namespace FolderBackup.Extension
{
    public static class StringExtension
    {
        public static string ToPath(this string text, char replacementChar = '_')
        {
            foreach (var invalidPathChar in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(invalidPathChar, replacementChar);
            }

            return text;
        }
    }
}
