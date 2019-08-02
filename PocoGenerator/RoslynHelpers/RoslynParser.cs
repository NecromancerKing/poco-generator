using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PocoGenerator.RoslynHelpers
{
    public static class RoslynParser
    {
        public static List<KeyValuePair<string, string>> ExtractFullClassesFromMainClass(string code)
        {
            List<KeyValuePair<string, string>> classList = new List<KeyValuePair<string, string>>();
            var ns = GetNamespace(code);
            var usings = GetUsings(code);
            var classes = ExtractClassesFromMainClass(code);
            foreach (var cls in classes)
            {
                var item = CreateClass(ns, usings, cls);
                classList.Add(item);
            }

            return classList;
        }
        public static string GetNamespace(string code)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var ns = (NamespaceDeclarationSyntax)root.Members[0];
            return ns.Name.ToString();
        }

        public static List<string> GetUsings(string code)
        {
            var usings = new List<string>();
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();

            var collector = new UsingCollector();
            collector.Visit(root);

            foreach (var directive in collector.Usings)
            {
                usings.Add(directive.Name.ToString());
            }

            return usings;
        }

        public static List<KeyValuePair<string, ClassDeclarationSyntax>> ExtractClassesFromMainClass(string code)
        {
            var classes = new List<KeyValuePair<string, ClassDeclarationSyntax>>();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var firstMember = root.Members[0];
            var nsDeclaration = (NamespaceDeclarationSyntax)firstMember;
            foreach (var cls in nsDeclaration.Members)
            {
                var clsDeclaration = (ClassDeclarationSyntax)cls;
                var item = new KeyValuePair<string, ClassDeclarationSyntax>(clsDeclaration.Identifier.Text, clsDeclaration);
                classes.Add(item);
            }

            return classes;
        }

        public static KeyValuePair<string, string> CreateClass(string nameSpace, List<string> usings, KeyValuePair<string, ClassDeclarationSyntax> classInfo)
        {
            // Create CompilationUnitSyntax
            var syntaxFactory = SyntaxFactory.CompilationUnit();

            // Add Usings
            foreach (var _using in usings)
            {
                syntaxFactory = syntaxFactory.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(_using)));
            }

            // Create the namespace
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(nameSpace)).NormalizeWhitespace();

            // Add the class to the namespace.
            @namespace = @namespace.AddMembers(classInfo.Value);

            // Add the namespace to the compilation unit.
            syntaxFactory = syntaxFactory.AddMembers(@namespace);

            // Normalize and get code as string.
            var code = syntaxFactory
                //.NormalizeWhitespace()
                .ToFullString();

            var result = new KeyValuePair<string, string>(classInfo.Key, code);
            return result;
        }
    }
}
