using System;
using LlvmParser;

namespace Nettens.CompilerCore {
	public class Compiler {
		public void LoadIR(string code) {
			IrParser.Parse(code);
		}
	}
}