using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Nettens.CompilerCore;

namespace Nettens.Cli {
	class Program {
		static void Main(string[] args) {
			var compiler = new Compiler();
			compiler.LoadIR(File.ReadAllText(args[0]));
			compiler.Compile("../tests/temp.cs");

			var tree = SyntaxFactory.ParseSyntaxTree(File.ReadAllText("../tests/temp.cs"));
			var compilation = CSharpCompilation.Create("NettensOutput")
				.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true))
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location))
				.AddSyntaxTrees(tree);
			var res = compilation.Emit(args[1]);
			if(!res.Success)
				foreach(var issue in res.Diagnostics)
					Console.WriteLine($"ID: {issue.Id} Message: {issue.GetMessage()} Location: {issue.Location.GetLineSpan()} Severity: {issue.Severity}");
		}
	}
}