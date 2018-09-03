using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using static System.Console;

namespace LlvmParser {
	public static class IrParser {
		public static void Parse(string code) {
			var inFunction = false;
			code.Split('\n').Select(x => x.Trim()).ForEach(line => {
				if(line.Contains(';')) line = line.Substring(0, line.IndexOf(';')).TrimEnd();
				if(line == "") return;

				if(inFunction) {
					if(line == "}") {
						inFunction = false;
						return;
					}
					WriteLine($"Instruction: {line}");
					return;
				}
				
				if(Regex.Match(line, @"^(\S+)\s*\=(.*)$") is Match match && match.Success) {
				} else if(Regex.Match(line, @"^target\s+(\S+)\s*=\s*(.*)$") is Match tmatch && tmatch.Success) {
				} else if(Regex.Match(line, @"^attributes\s+#[0-9]+\s*=\s*\{\s*.*\s*\}$") is Match amatch && amatch.Success) {
				} else if(Regex.Match(line, @"^define\s+(\S+)\s+@(\S+)\s*\((.*)\)(\s+#[0-9]+)*\s*\{$") is Match dmatch && dmatch.Success) {
					inFunction = true;
				} else
					WriteLine($"UNHANDLED {line}");
			});
		}

		static void ParseInstruction(string code) {
			
		}
	}
}