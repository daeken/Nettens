using System;
using System.IO;
using Nettens.CompilerCore;

namespace Nettens.Cli {
	class Program {
		static void Main(string[] args) {
			var compiler = new Compiler();
			compiler.LoadIR(File.ReadAllText("../tests/if.s"));
		}
	}
}