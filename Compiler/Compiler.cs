using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LlvmParser;
using MoreLinq;
using PrettyPrinter;

namespace Nettens.CompilerCore {
	public class Compiler {
		readonly List<IrModule> Modules = new List<IrModule>();

		public void LoadIR(string code) {
			var module = IrParser.Parse(code);
			//module.Print();
			Modules.Add(module);
		}

		int Indentation;
		StreamWriter SW;

		public void Compile(string outfn) {
			SW = new StreamWriter(outfn);
			Write("using System;");
			Write("namespace NettensOutput {");
			Indentation++;
			Write("public static unsafe class Module {");
			Indentation++;

			foreach(var module in Modules)
				foreach(var func in module.Functions)
					Compile(func);
			
			Indentation--;
			Write("}");
			Indentation--;
			Write("}");
			SW.Dispose();
		}

		void Write(string data) =>
			SW.WriteLine((Indentation > 0 ? new string('\t', Indentation) : "") + data);
		
		static readonly Dictionary<string, string> BinaryOps = new Dictionary<string, string> {
			["add"] = "+", 
			["mul"] = "*", 
			["sdiv"] = "/", 
			["shl"] = "<<", 
		};

		static readonly Dictionary<string, string> CompareOps = new Dictionary<string, string> {
			["sgt"] = ">"
		};

		void Compile(IrFunction func) {
			Write($"public static {From(func.ReturnType)} {func.Name}({string.Join(", ", func.ParameterTypes.Select((pt, i) => $"{From(pt)} _{i}"))}) {{");
			Indentation++;

			var blockPhiResolvers = new Dictionary<string, List<string>>();
			foreach(var block in func.Blocks)
				blockPhiResolvers[block.Name] = new List<string>();

			foreach(var block in func.Blocks) {
				foreach(var inst in block.Instructions)
					if(inst is PhiInst phi) {
						Write($"{From(phi.Output.Type)} {Rewrite(phi.Output)};");
						foreach(var (bname, value) in phi.Incoming)
							blockPhiResolvers[bname].Add($"{Rewrite(phi.Output)} = {Rewrite(value)};");
					}
			}

			foreach(var block in func.Blocks) {
				bool finished = false;
				void FinishBlock() {
					if(finished) return;
					finished = true;
					blockPhiResolvers[block.Name].ForEach(Write);
				}

				Write($"{RenameBlock(block.Name)}:");
				Indentation++;
				foreach(var inst in block.Instructions) {
					switch(inst) {
						case AllocaInst a:
							Write($"var {Rewrite(a.Output)} = stackalloc {From(a.AllocationType)}[{Rewrite(a.AllocationRank)}];");
							break;
						case BinaryInst b:
							Write($"var {Rewrite(b.Output)} = {Rewrite(b.A)} {BinaryOps[b.Op]} {Rewrite(b.B)};");
							break;
						case BrIfInst bri:
							FinishBlock();
							Write($"if({Rewrite(bri.Condition)}) goto {RenameBlock(bri.If)};");
							Write($"else goto {RenameBlock(bri.Else)};");
							break;
						case BrInst br:
							FinishBlock();
							Write($"goto {RenameBlock(br.Target)};");
							break;
						case CallInst call:
							Write($"var {Rewrite(call.Output)} = {call.Target}({string.Join(", ", call.Parameters.Select(Rewrite))});");
							break;
						case IcmpInst icmp:
							Write($"var {Rewrite(icmp.Output)} = {Rewrite(icmp.A)} {CompareOps[icmp.Predicate]} {Rewrite(icmp.B)};");
							break;
						case LoadInst load:
							Write($"var {Rewrite(load.Output)} = *{Rewrite(load.Pointer)};");
							break;
						case PhiInst phi:
							break;
						case ReturnInst ret:
							FinishBlock();
							Write(ret.Value == null ? "return;" : $"return {Rewrite(ret.Value)};");
							break;
						case SelectInst select:
							Write($"var {Rewrite(select.Output)} = {Rewrite(select.Compare)} ? {Rewrite(select.A)} : {Rewrite(select.B)};");
							break;
						case StoreInst store:
							Write($"*{Rewrite(store.Pointer)} = {Rewrite(store.Value)};");
							break;
						default:
							Console.Write("Unknown instruction: ");
							inst.Print();
							break;
					}
				}
				FinishBlock();
				Indentation--;
			}
			
			Indentation--;
			Write("}");
		}

		string Rewrite(IrOperand op) => Rewrite(op.Name);

		string Rewrite(string name) {
			if(name.StartsWith("%"))
				return $"_{name.Substring(1)}";
			return name;
		}

		string RenameBlock(string name) =>
			$"_{name.Replace("%", "_")}";

		string From(IrType type) => FromType(type.Name);

		string FromType(string type) {
			if(type.EndsWith("*")) return FromType(type.Substring(0, type.Length - 1)) + "*";
			switch(type) {
				case "i1": return "bool";
				case "i8": return "sbyte";
				case "i32": return "int";
			}
			return type;
		}
	}
}