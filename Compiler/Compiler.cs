using System;
using System.Collections.Generic;
using LlvmParser;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MoreLinq;
using PrettyPrinter;

namespace Nettens.CompilerCore {
	public class Compiler {
		readonly List<IrModule> Modules = new List<IrModule>();

		TypeSystem TS;
		
		public void LoadIR(string code) {
			var module = IrParser.Parse(code);
			//module.Print();
			Modules.Add(module);
		}

		public void Compile(string outfn) {
			var asm = AssemblyDefinition.CreateAssembly(
				new AssemblyNameDefinition(Guid.NewGuid().ToString(), new Version(1, 0, 0, 0)), "NettensModule",
				ModuleKind.Dll);
			var module = asm.MainModule;
			TS = module.TypeSystem;
			var type = new TypeDefinition("Nettens", "MainType", TypeAttributes.Class);
			module.Types.Add(type);

			foreach(var irm in Modules) {
				foreach(var func in irm.Functions) {
					type.Methods.Add(CompileFunction(func));
				}
			}
			
			module.Write(outfn);
		}

		MethodDefinition CompileFunction(IrFunction func) {
			var method = new MethodDefinition(func.Name, MethodAttributes.Public | MethodAttributes.Static, FromIrType(func.ReturnType));
			func.ParameterTypes.ForEach((pt, i) => method.Parameters.Add(new ParameterDefinition(FromIrType(pt))));
			var il = method.Body.GetILProcessor();
			il.Append(il.Create(OpCodes.Brtrue));
			return method;
		}

		TypeReference FromIrType(IrType type) {
			switch(type.Name) {
				case "i32":
					return TS.Int32;
				default:
					type.Print();
					throw new NotSupportedException();
			}
		}

		TypeReference FromType<T>() {
			var a = AssemblyDefinition.ReadAssembly(typeof(T).Assembly.Location);
			return a.MainModule.ImportReference(typeof(T));
		}
	}
}