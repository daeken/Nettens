using System;
using Nettens.BitCode;

namespace Nettens.CompilerCore {
	public class Compiler {
		public void LoadBC(byte[] data) {
			var bcr = new BitCodeReader(data);
		}
	}
}