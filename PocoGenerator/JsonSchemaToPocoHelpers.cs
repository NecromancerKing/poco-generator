using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using PocoGenerator.RoslynHelpers;

namespace PocoGenerator
{
    public static class JsonSchemaToPocoHelpers
    {
        private static string _ConvertJsonSchemaFileToPoco(string filename, string nameSpace)
        {
            var schema = JsonSchema.FromFileAsync(filename);
            CSharpGeneratorSettings settings = new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco,
                SchemaType = SchemaType.JsonSchema,
                GenerateJsonMethods = false,
                HandleReferences = true,
                Namespace = nameSpace
            };
            var generator = new CSharpGenerator(schema.Result, settings);
            var file = generator.GenerateFile();
            file = file.Replace(" partial", "");
            var lines = file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            lines = lines
                .Where(line =>
                    !line.Contains("System.CodeDom.Compiler.GeneratedCode") &&
                    !line.Contains("Newtonsoft.Json") &&
                    !line.Contains("#pragma") &&
                    !line.Contains("auto-generated") &&
                    !string.IsNullOrWhiteSpace(line.Trim()) &&
                    !line.Contains("//---"))
                .Select(t => t.TrimEnd())
                .ToList();

            string poco = string.Join(Environment.NewLine, lines.ToArray());

            return poco;
        }

        public static List<KeyValuePair<string, string>> ConvertJsonSchemaFileToPoco(string filename, string nameSpace)
        {
            var pocos = _ConvertJsonSchemaFileToPoco(filename, nameSpace);
            var fullClasses = RoslynParser.ExtractFullClassesFromMainClass(pocos);
            return fullClasses;
        }

        public static void WritePocoFiles(List<KeyValuePair<string, string>> pocos, string outputDir)
        {
            foreach (var poco in pocos)
            {
                File.Create(Path.Combine(outputDir, poco.Key + ".cs"));
            }
        }
    }
}
