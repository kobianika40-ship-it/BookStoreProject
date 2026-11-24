using BookStore.Enums;

namespace BookStore.Helpers
{
    internal static class FileHelper<T>
    {
        public static void WriteInFile(FILE_PATHS filePath, T entity, bool append = false)
        {
            using (StreamWriter writer = new StreamWriter(
                PathHelper.GetPath(filePath), append: append))
            {
                writer.WriteLine(entity);
            }
        }

        public static void WriteInFile(FILE_PATHS filePath, List<T> values, bool append = false)
        {
            using (StreamWriter writer = new StreamWriter(
                PathHelper.GetPath(filePath), append: append))
            {
                foreach (T entity in values)
                {
                    writer.WriteLine(entity);
                }
            }
        }

        public static void ReadFromFile(FILE_PATHS filePath)
        {
            using (StreamReader reader = new StreamReader(
                PathHelper.GetPath(filePath)))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public static bool DeleteFile(FILE_PATHS filePath)
        {
            string path = PathHelper.GetPath(filePath);

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }
    }
}
