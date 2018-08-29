using System;
using System.Runtime.InteropServices;

namespace Nettens.BitCode {
	public class BitReader {
		readonly byte[] Data;
		int BitPosition;
		int BytePosition;
		
		public BitReader(byte[] data) => Data = data;

		public void NextBit() {
			if(++BitPosition == 8) {
				BitPosition = 0;
				++BytePosition;
			}
		}

		public uint ReadBit() {
			var val = ((uint) Data[BytePosition] >> BitPosition) & 1;
			NextBit();
			return val;
		}

		public uint PeekBit() => ((uint) Data[BytePosition] >> BitPosition) & 1;

		public T Read<T>(int bits = 0) where T : struct {
			bits = bits == 0 ? Marshal.SizeOf<T>() * 8 : bits;

			var val = 0UL;
			for(var i = 0; i < bits; ++i)
				val |= (ulong) ReadBit() << i;
			var tv = new T();
			unchecked {
				switch(tv) {
					case byte _: return (T) (object) (byte) val;
					case sbyte _: return (T) (object) (sbyte) val;
					case ushort _: return (T) (object) (ushort) val;
					case short _: return (T) (object) (short) val;
					case uint _: return (T) (object) (uint) val;
					case int _: return (T) (object) (int) val;
					case ulong _: return (T) (object) val;
					case long _: return (T) (object) (long) val;
					default: throw new NotSupportedException();
				}
			}
		}
	}
}