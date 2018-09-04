#include <llvm/Pass.h>
#include <llvm/ADT/SmallVector.h>
#include <llvm/IR/BasicBlock.h>
#include <llvm/IR/CallingConv.h>
#include <llvm/IR/Constants.h>
#include <llvm/IR/DerivedTypes.h>
#include <llvm/IR/Function.h>
#include <llvm/IR/GlobalVariable.h>
#include <llvm/IR/InlineAsm.h>
#include <llvm/IR/Instructions.h>
#include <llvm/IR/InstVisitor.h>
#include <llvm/IR/LLVMContext.h>
#include <llvm/IR/Module.h>
#include <llvm/IR/Operator.h>
#include <llvm/IRReader/IRReader.h>
#include <llvm/Support/SourceMgr.h>
#include <llvm/Support/raw_ostream.h>
#include <iostream>
using namespace llvm;
using namespace std;

int indentation = 0;

string strType(Type *type) {
    string temp;
    raw_string_ostream rso(temp);
    type->print(rso);
    return rso.str();
}

string getName(Value &bb) {
    if(bb.hasName())
        return bb.getName().str();
    string temp;
    raw_string_ostream rso(temp);
    bb.printAsOperand(rso, false);
    return rso.str();
}

void output(string val) {
    for(auto i = 0; i < indentation; ++i)
        cout << "\t";
    cout << ";" << val.length() << ":" << val << "^" << endl;
}

template<typename Func>
void output(string type, string value, Func cb) {
    output(type);
    indentation++;
    output(value);
    cb();
    indentation--;
    output("$$end");
}

void output(Type *type) {
    output("type", strType(type), [] {});
}

void outputOperand(Value *val) {
    output("operand", getName(*val), [&] {
        output(val->getType());
    });
}

class OutputVisitor : public InstVisitor<OutputVisitor> {
public:
    void _output(Instruction &inst) {
        visit(inst);
    }

    void visitAllocaInst(AllocaInst &alloca) {
        output("instruction", "alloca", [&] {
            outputOperand(&alloca);
            output(alloca.getAllocatedType());
            outputOperand(alloca.getArraySize());
        });
    }

    void visitBranchInst(BranchInst &branch) {
        if(branch.isConditional())
            output("instruction", "br.if", [&] {
                outputOperand(branch.getOperand(0));
                output("target", getName(*branch.getSuccessor(0)), [] {});
                output("target", getName(*branch.getSuccessor(1)), [] {});
            });
        else
            output("instruction", "br", [&] {
                output("target", getName(*branch.getSuccessor(0)), [] {});
            });
    }

    void visitICmpInst(ICmpInst &icmp) {
        auto pred = icmp.isSigned()
            ? icmp.getSignedPredicate()
            : icmp.getUnsignedPredicate();
        output("instruction", "icmp", [&] {
            output("predicate", ICmpInst::getPredicateName(pred).str(), [] {});
            outputOperand(&icmp);
            outputOperand(icmp.getOperand(0));
            outputOperand(icmp.getOperand(1));
        });
    }

    void visitLoadInst(LoadInst &load) {
        output("instruction", "load", [&] {
            outputOperand(&load);
            outputOperand(load.getPointerOperand());
        });
    }

    void visitStoreInst(StoreInst &store) {
        output("instruction", "store", [&] {
            outputOperand(store.getPointerOperand());
            outputOperand(store.getValueOperand());
        });
    }

    void visitBinaryOperator(BinaryOperator &b) {
        output("instruction", "binary", [&] {
            output("opcode", b.getOpcodeName(), [] {});
            if(auto *ob = dyn_cast<OverflowingBinaryOperator>(&b)) {
                if(ob->hasNoSignedWrap())
                    output("flag", "nsw", [] {});
                if(ob->hasNoUnsignedWrap())
                    output("flag", "nuw", [] {});
            }
            outputOperand(&b);
            outputOperand(b.getOperand(0));
            outputOperand(b.getOperand(1));
        });
    }

    void visitCallInst(CallInst &call) {
        output("instruction", "call", [&] {
            outputOperand(&call);
            //if(call.isTailCall())
            //    output("flag", "tail", [] {});
            output("target", call.getCalledFunction()->getName().str(), [] {});
            for(auto &o : call.arg_operands()) {
                outputOperand(o);
            }
        });
    }

    void visitPHINode(PHINode &phi) {
        output("instruction", "phi", [&] {
            outputOperand(&phi);
            for(auto i = 0; i < phi.getNumIncomingValues(); ++i)
                output(getName(*phi.getIncomingBlock(i)), getName(*phi.getIncomingValue(i)), [] {});
        });
    }

    void visitReturnInst(ReturnInst &ret) {
        output("instruction", "ret", [&] {
            if(ret.getReturnValue() != nullptr)
                outputOperand(ret.getReturnValue());
        });
    }

    void visitSelectInst(SelectInst &sel) {
        output("instruction", "select", [&] {
            outputOperand(&sel);
            outputOperand(sel.getOperand(0));
            outputOperand(sel.getOperand(1));
            outputOperand(sel.getOperand(2));
        });
    }
};

void output(Instruction &inst) {
    /*string temp;
    raw_string_ostream rso(temp);
    inst.print(rso, false);
    output("inst", rso.str(), [] {});*/
    OutputVisitor v;
    v._output(inst);
}

int main(int argc, char **argv) {
    if(argc < 2) {
        cout << "Usage: " << argv[0] << " <input.bc>" << endl;
        return 1;
    }
    LLVMContext context;
    SMDiagnostic error;
    auto module = parseIRFile(argv[1], error, context);
    if(!module) {
        cout << "Error from parseIRFile: " << error.getMessage().str() << endl;
        return 1;
    }

    for(auto &func : module->getFunctionList()) {
        auto name = func.getName().str();
        output("function", name, [&] {
            auto funcType = func.getFunctionType();
            output(funcType->getReturnType());
            auto numParams = funcType->getFunctionNumParams();
            for(auto i = 0U; i < numParams; ++i)
                output(funcType->getFunctionParamType(i));
            for(auto &bb : func) {
                output("block", getName(bb), [&] {
                    for(auto &inst : bb)
                        output(inst);
                });
            }
        });
    }

    return 0;
}