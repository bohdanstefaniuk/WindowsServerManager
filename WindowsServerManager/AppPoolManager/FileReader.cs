using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppPoolManager
{
    public class FileReader
    {
        public async Task<string> ReadTextFileAsync(string path)
        {
            try
            {
                var x = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var streamReader = new StreamReader(x))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
            catch (Exception e)
            {
                // TODO refactor
                Debug.WriteLine(e.Message);
                return $"Ошибка при чтении: {e.Message}";
            }
        }

        public string ReadTextFile(string path)
        {
            try
            {
                using (var streamReader = new StreamReader(path))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                // TODO refactor
                Debug.WriteLine(e.Message);
                return $"Ошибка при чтении: {e.Message}";
            }
        }
    }
}
