using System;
using System.IO;
using System.Reflection;

namespace PocoGenerator
{
    class Program
    {
        private static string _namespace;
        private static string _foldername;
        private static string _filename;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter generation parameters.");
                return;
            }

            _namespace = args[0];
            _foldername = args[1];
            _filename = args[2];

            if (string.IsNullOrEmpty(_namespace))
            {
                Console.WriteLine("Namespace is not supported");
                return;
            }

            if (string.IsNullOrEmpty(_foldername))
            {
                Console.WriteLine("Output folder name is not supported");
                return;
            }

            if (string.IsNullOrEmpty(_filename))
            {
                Console.WriteLine("Json schema file name is not supported");
                return;
            }

            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string outputDir = Path.Combine(assemblyLocation, _foldername);
            string filename = Path.Combine(assemblyLocation, _filename);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var pocos = JsonSchemaToPocoHelpers.ConvertJsonSchemaFileToPoco(filename, _namespace);
            JsonSchemaToPocoHelpers.WritePocoFiles(pocos, outputDir);
        }
    }
}
