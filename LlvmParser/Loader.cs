using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using static System.Console;

namespace LlvmParser {
	class MfNode : IEnumerable<MfNode> {
		internal string Name, Value;
		readonly List<MfNode> Children = new List<MfNode>();

		public void Add(MfNode child) => Children.Add(child);

		public MfNode this[int i] => Children[i];

		public IEnumerator<MfNode> GetEnumerator() => Children.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	public static class IrParser {
		public static IrModule Parse(string code) {
			var cur = new MfNode();
			var stack = new Stack<MfNode>();

			var tokens = Tokenize(code).ToList();
			for(var i = 0; i < tokens.Count;) {
				if(tokens[i] == "$$end") {
					i++;
					cur = stack.Pop();
				} else {
					var next = new MfNode { Name = tokens[i++], Value = tokens[i++] };
					stack.Push(cur);
					cur.Add(next);
					cur = next;
				}
			}

			return new IrModule { Functions = cur.Select(ParseFunction).ToArray() };
		}

		static IrFunction ParseFunction(MfNode node) {
			var types = node.Where(x => x.Name == "type").Select(ParseType).ToList();
			Debug.Assert(types.Count > 0);
			
			return new IrFunction {
				Name = node.Value, 
				ReturnType = types[0], 
				ParameterTypes = types.Skip(1).ToArray(), 
				Blocks = node.Where(x => x.Name == "block").Select(ParseBlock).ToArray()
			};
		}

		static IrBlock ParseBlock(MfNode node) {
			return new IrBlock {
				Name = node.Value, 
				Instructions = node.Select(ParseInstruction).ToArray()
			};
		}

		static IrOperand ParseOperand(MfNode node) {
			return new IrOperand {
				Name = node.Value, 
				Type = ParseType(node[0])
			};
		}

		static IrInst ParseInstruction(MfNode node) {
			switch(node.Value) {
				case "alloca":
					return new AllocaInst {
						Output = ParseOperand(node[0]), 
						AllocationRank = ParseOperand(node[2]), 
						AllocationType = ParseType(node[1])
					};
				case "binary":
					var operands = node.Where(x => x.Name == "operand").ToList();
					var flags = BinaryFlags.None;
					foreach(var elem in node)
						if(elem.Name == "flag" && elem.Value == "nsw")
							flags |= BinaryFlags.Nsw;
						else if(elem.Name == "flag" && elem.Value == "nuw")
							flags |= BinaryFlags.Nuw;
					return new BinaryInst {
						Op = node[0].Value, 
						Flags = flags, 
						Output = ParseOperand(operands[0]), 
						A = ParseOperand(operands[1]), 
						B = ParseOperand(operands[2]) 
					};
				case "br":
					return new BrInst {
						Target = node[0].Value
					};
				case "br.if":
					return new BrIfInst {
						Condition = ParseOperand(node[0]), 
						If = node[1].Value, 
						Else = node[2].Value
					};
				case "call":
					return new CallInst {
						Output = ParseOperand(node[0]), 
						Target = node[1].Value, 
						Parameters = node.Skip(2).Select(ParseOperand).ToArray()
					};
				case "icmp":
					return new IcmpInst {
						Predicate = node[0].Value, 
						Output = ParseOperand(node[1]), 
						A = ParseOperand(node[2]), 
						B = ParseOperand(node[3])
					};
				case "load":
					return new LoadInst {
						Output = ParseOperand(node[0]), 
						Pointer = ParseOperand(node[1])
					};
				case "phi":
					return new PhiInst {
						Output = ParseOperand(node[0]), 
						Incoming = node.Skip(1).Select(x => (x.Name, x.Value)).ToArray()
					};
				case "ret":
					return new ReturnInst {
						Value = ParseOperand(node[0])
					};
				case "select":
					return new SelectInst {
						Output = ParseOperand(node[0]), 
						Compare = ParseOperand(node[1]), 
						A = ParseOperand(node[2]), 
						B = ParseOperand(node[3])
					};
				case "store":
					return new StoreInst {
						Pointer = ParseOperand(node[0]), 
						Value = ParseOperand(node[1])
					};
				default:
					WriteLine(node.Value);
					return null;
			}
		}

		static IrType ParseType(MfNode node) {
			return new IrType { Name = node.Value };
		}

		static IEnumerable<string> Tokenize(string code) {
			for(var i = 0; i < code.Length; ++i) {
				if(code[i] != ';')
					i = code.IndexOf(';', i);
				if(i == -1) break;
				var lend = code.IndexOf(':', i);
				var len = int.Parse(code.Substring(i + 1, lend - i - 1));
				var data = code.Substring(lend + 1, len);
				i = lend + len + 1;
				Debug.Assert(code[i] == '^');
				yield return data;
			}
		}
	}
}