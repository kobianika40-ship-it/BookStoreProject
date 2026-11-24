using BookStore.Enums;

namespace BookStore.Helpers
{
    internal static class PathHelper
    {
        private static readonly string[] _fileNames = new string[]
        {
            "Users.json",
            "Library.txt"
        };

        public static string GetPath(FILE_PATHS filePath)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (string.IsNullOrWhiteSpace(desktop))
            {
                desktop = AppContext.BaseDirectory;
            }

            string folder = Path.Combine(desktop, "BookStoreData");
            Directory.CreateDirectory(folder);

            return Path.Combine(folder, _fileNames[Convert.ToInt32(filePath)]);
        }
    }
}
