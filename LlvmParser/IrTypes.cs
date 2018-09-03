using System;

namespace LlvmParser {
	public class IrType {
		public string Name;
	}

	public class IrValue {
		public string Ident;
	}

	public class IrOperand {
		public string Name;
		public IrType Type;
	}

	public abstract class IrInst {
	}

	public class AllocaInst : IrInst {
		public IrOperand Output, AllocationRank;
		public IrType AllocationType;
	}

	[Flags]
	public enum BinaryFlags {
		None = 0, 
		Nuw = 1, 
		Nsw = 2
	}

	public class BinaryInst : IrInst {
		public BinaryFlags Flags;
		public string Op;
		public IrOperand Output, A, B;
	}

	public class BrInst : IrInst {
		public string Target;
	}

	public class BrIfInst : IrInst {
		public IrOperand Condition;
		public string If, Else;
	}

	public class CallInst : IrInst {
		public IrOperand Output;
		public string Target;
		public IrOperand[] Parameters;
	}

	public class IcmpInst : IrInst {
		public string Predicate;
		public IrOperand Output, A, B;
	}

	public class LoadInst : IrInst {
		public IrOperand Output, Pointer;
	}

	public class ReturnInst : IrInst {
		public IrOperand Value;
	}

	public class StoreInst : IrInst {
		public IrOperand Pointer, Value;
	}

	public class IrBlock {
		public string Name;
		public IrInst[] Instructions;
	}
	
	public class IrFunction {
		public string Name;
		public IrType ReturnType;
		public IrType[] ParameterTypes;
		public IrBlock[] Blocks;
	}

	public class IrModule {
		public IrFunction[] Functions;
	}
}