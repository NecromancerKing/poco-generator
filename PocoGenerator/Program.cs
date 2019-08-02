using System;
using System.IO;
using System.Threading.Tasks;

namespace PocoGenerator
{
    class Program
    {
        private static string _namespace;
        private static string _folderpath;
        private static string _filename;
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter generation parameters.");
                return;
            }

            _namespace = args[0];
            _filename = args[1];
            _folderpath = args[2];

            if (string.IsNullOrEmpty(_namespace))
            {
                Console.WriteLine("Namespace is not supported");
                return;
            }

            if (string.IsNullOrEmpty(_filename))
            {
                Console.WriteLine("Json schema file name is not supported");
                return;
            }

            if (string.IsNullOrEmpty(_folderpath))
            {
                Console.WriteLine("Output folder name is not supported");
                return;
            }

            if (!Directory.Exists(_folderpath))
            {
                Directory.CreateDirectory(_folderpath);
            }

            var pocos =  await JsonSchemaToPocoHelpers.ConvertJsonSchemaFileToPocoAsync(_filename, _namespace).ConfigureAwait(false);
            JsonSchemaToPocoHelpers.WritePocoFiles(pocos, _folderpath);
            Console.WriteLine("done!");
        }
    }
}
