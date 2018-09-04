using System;
using System.IO;
using Nettens.CompilerCore;

namespace Nettens.Cli {
	class Program {
		static void Main(string[] args) {
			var compiler = new Compiler();
			compiler.LoadIR(File.ReadAllText(args[0]));
			compiler.Compile(args[1]);
		}
	}
}