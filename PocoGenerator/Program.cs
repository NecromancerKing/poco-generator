using CommandLine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PocoGenerator
{
    class Program
    {
        private static string _namespace;
        private static string _jsonPath;
        private static string _folderPath;
        static async Task Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts =>
                {
                    _namespace = opts.Namespace;
                    _jsonPath = opts.JsonPath;
                    _folderPath = opts.FolderPath;
                })
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors)
                    {
                        if (error.Tag != ErrorType.HelpRequestedError && error.Tag != ErrorType.VersionRequestedError)
                            Console.WriteLine(error);
                    }
                });

            // help mode, version mode, or hit errors
            if (result.Tag == ParserResultType.NotParsed) return;

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            var pocos =  await JsonSchemaToPocoHelpers.ConvertJsonSchemaFileToPocoAsync(_jsonPath, _namespace).ConfigureAwait(false);
            JsonSchemaToPocoHelpers.WritePocoFiles(pocos, _folderPath);
            Console.WriteLine("done!");
        }
    }
}
