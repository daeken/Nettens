using System;
using static System.Console;

namespace Nettens.BitCode {
	public class BitCodeReader {
		readonly BitReader Br;
		public BitCodeReader(byte[] data) {
			Br = new BitReader(data);
		}
	}
}