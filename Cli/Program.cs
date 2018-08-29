using System;
using System.IO;
using Nettens.CompilerCore;

namespace Nettens.Cli {
	class Program {
		static void Main(string[] args) {
			var compiler = new Compiler();
			compiler.LoadBC(File.ReadAllBytes("../tests/add.bc"));
		}
	}
}