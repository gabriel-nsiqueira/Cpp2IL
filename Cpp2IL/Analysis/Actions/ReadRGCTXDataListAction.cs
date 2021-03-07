﻿using Cpp2IL.Analysis.ResultModels;
using LibCpp2IL.PE;
using Mono.Cecil.Cil;
using Instruction = Iced.Intel.Instruction;

namespace Cpp2IL.Analysis.Actions
{
    public class ReadRGCTXDataListAction : BaseAction
    {
        private Il2CppClassIdentifier? _klass;
        private Il2CppRGCTXDefinition[]? _rgctxs;
        private string? _destReg;
        private ConstantDefinition? _constantMade;

        public ReadRGCTXDataListAction(MethodAnalysis context, Instruction instruction) : base(context, instruction)
        {
            var klassConst = context.GetConstantInReg(Utils.GetRegisterNameNew(instruction.MemoryBase));
            _klass = klassConst?.Value as Il2CppClassIdentifier;

            if (_klass == null)
                return;

            _rgctxs = _klass.backingType.RGCTXs;

            if (_rgctxs == null)
                return;

            _destReg = Utils.GetRegisterNameNew(instruction.Op0Register);
            _constantMade = context.MakeConstant(typeof(Il2CppRGCTXArray), new Il2CppRGCTXArray
            {
                Rgctxs = _rgctxs,
            }, reg: _destReg);
        }

        public override Mono.Cecil.Cil.Instruction[] ToILInstructions(MethodAnalysis context, ILProcessor processor)
        {
            throw new System.NotImplementedException();
        }

        public override string? ToPsuedoCode()
        {
            throw new System.NotImplementedException();
        }

        public override string ToTextSummary()
        {
            return $"Reads RGCTX data for class {_klass?.backingType.FullName} which has {_rgctxs?.Length} entries/s and stores in new constant {_constantMade?.Name} in register {_destReg}";
        }
    }
}