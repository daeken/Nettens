using System;
using NettensOutput;

namespace TestApp {
	class Program {
		static void Main(string[] args) {
			/*Console.WriteLine($"5 + 6 == {NettensOutput.Module.add(5, 6)} == {5 + 6}");
			Console.WriteLine($"5 * 6 == {NettensOutput.Module.mul(5, 6)} == {5 * 6}");
			Console.WriteLine($"5 * 6 + 7 == {NettensOutput.Module.fused_muladd(5, 6, 7)} == {5 * 6 + 7}");*/
			Console.WriteLine(Module.test(5, 6));
		}
	}
}