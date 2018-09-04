using System;
namespace NettensOutput {
	public static unsafe class Module {
		public static int test(int _0, int _1) {
			__2:
			var _3 = stackalloc int[1];
			var _4 = stackalloc int[1];
			var _5 = stackalloc int[1];
			*_4 = _0;
			*_5 = _1;
			var _6 = *_4;
			var _7 = _6 > 0;
			if(_7) goto __8;
			else goto __12;
			__8:
			var _9 = *_4;
			var _10 = *_5;
			var _11 = _9 * _10;
			*_3 = _11;
			goto __16;
			__12:
			var _13 = *_4;
			var _14 = *_5;
			var _15 = _13 + _14;
			*_3 = _15;
			goto __16;
			__16:
			var _17 = *_3;
			return _17;
		}
	}
}
