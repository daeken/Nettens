using System;
using System.Diagnostics;
using static System.Console;

namespace Nettens.BitCode {
	public class BitCodeReader {
		readonly BitReader Br;
		public BitCodeReader(byte[] data) {
			Br = new BitReader(data);
			Debug.Assert(Br.Read<uint>() == 0xDEC04342);
		}
	}
}