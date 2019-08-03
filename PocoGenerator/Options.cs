using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace PocoGenerator
{
    public class Options
    {
        [Option(Required = true, HelpText = "Namespace for the Poco classes")]
        public string Namespace { get; set; }
        [Option(Required = true, HelpText = "Path to the json schema file")]
        public string JsonPath { get; set; }
        [Option(Required = true, HelpText = "Path to the output folder")]
        public string FolderPath { get; set; }

        [Usage(ApplicationAlias = "PocoGenerator")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Absolute Path",
                    new Options
                    {
                        Namespace = "Base.Child", JsonPath = "C:\\SomeFolder\\schema.json",
                        FolderPath = "C:\\Folder\\NamedFolder"
                    });
                yield return new Example("Relative Path",
                    new Options
                    {
                        Namespace = "Base.Child",
                        JsonPath = "..\\schema.json",
                        FolderPath = "..\\..\\NamedFolder"
                    });
            }
        }
    }
}
