using System;
using System.Collections.Generic;
using LlvmParser;
using PrettyPrinter;

namespace Nettens.CompilerCore {
	public class Compiler {
		public void LoadIR(string code) {
			var module = IrParser.Parse(code);
			module.Print();
		}
	}
}