using System;
namespace NettensOutput {
	public static unsafe class Module {
		public static int add(int _0, int _1) {
			__2:
				var _3 = stackalloc int[1];
				var _4 = stackalloc int[1];
				*_3 = _0;
				*_4 = _1;
				var _5 = *_3;
				var _6 = *_4;
				var _7 = _5 + _6;
				return _7;
		}
		public static int mul(int _0, int _1) {
			__2:
				var _3 = stackalloc int[1];
				var _4 = stackalloc int[1];
				*_3 = _0;
				*_4 = _1;
				var _5 = *_3;
				var _6 = *_4;
				var _7 = _5 * _6;
				return _7;
		}
		public static int fused_muladd(int _0, int _1, int _2) {
			__3:
				var _4 = stackalloc int[1];
				var _5 = stackalloc int[1];
				var _6 = stackalloc int[1];
				*_4 = _0;
				*_5 = _1;
				*_6 = _2;
				var _7 = *_4;
				var _8 = *_5;
				var _9 = mul(_7, _8);
				var _10 = *_6;
				var _11 = add(_9, _10);
				return _11;
		}
	}
}
