using System;
namespace NettensOutput {
	public static unsafe class Module {
		public static int dbl(int _0) {
			__1:
				var _2 = stackalloc int[1];
				*_2 = _0;
				var _3 = *_2;
				var _4 = *_2;
				var _5 = _3 + _4;
				return _5;
		}
		public static int test(int _0, int _1) {
			__2:
				var _3 = stackalloc int[1];
				var _4 = stackalloc int[1];
				var _5 = stackalloc int[1];
				*_3 = _0;
				*_4 = _1;
				var _6 = *_3;
				var _7 = *_4;
				var _8 = _6 + _7;
				*_5 = _8;
				var _9 = *_3;
				var _10 = _9 > 0;
				if(_10) goto __11;
				else goto __15;
			__11:
				var _12 = *_3;
				var _13 = *_5;
				var _14 = _13 / _12;
				*_5 = _14;
				goto __15;
			__15:
				var _16 = *_4;
				var _17 = dbl(_16);
				var _18 = *_5;
				var _19 = _18 + _17;
				*_5 = _19;
				var _20 = *_5;
				return _20;
		}
	}
}
