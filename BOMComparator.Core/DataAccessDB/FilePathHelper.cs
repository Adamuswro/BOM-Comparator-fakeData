using System.IO;

namespace BOMComparator.Core.DataAccessDB
{
    /// <summary>
    /// Generator of the file paths - helpfull during development on the different machines.
    /// </summary>
    public class FilePathHelper
    {
        /// <summary>
        /// Returns the path to file which is in folder in main project directory.
        /// </summary>
        /// <param name="folderName">Folder name - folder needs to be in same directory like project.</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GeneratePath(string folderName, string fileName)
        {
            var projectDirectory = GetProjectDirectory();
            var result = Path.Combine(projectDirectory, folderName, fileName);
            return result;
        }

        public static string GetExternalFilesPath(string fileName)
        {
            var result = Path.Combine(GetExternalFilesPath(), fileName);
            return result;
        }

        public static string GetExternalFilesPath()
        {
            var result = Path.Combine(GetProjectDirectory(), "ExternalFiles");
            return result;
        }

        private static string GetProjectDirectory()
        {
            var applicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var result = Path.GetFullPath(Path.Combine(applicationPath, "..", ".."));
            return result;
        }
    }
}