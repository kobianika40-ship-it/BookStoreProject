using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BookStore.Enums;

namespace BookStore.Helpers
{
    internal static class JsonHelper<T>
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public async static Task WriteInJsonAsync(FILE_PATHS path, List<T> values)
        {
            await using FileStream stream =
                File.Create(PathHelper.GetPath(path));

            await JsonSerializer
                .SerializeAsync(stream, values, _options);
        }

        public async static Task<List<T>> ReadFromJsonAsync(FILE_PATHS path)
        {
            string filePath = PathHelper.GetPath(path);

            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            await using FileStream stream = File.OpenRead(filePath);

            List<T>? result = await JsonSerializer
                .DeserializeAsync<List<T>>(stream, _options);

            return result ?? new List<T>();
        }
    }
}
