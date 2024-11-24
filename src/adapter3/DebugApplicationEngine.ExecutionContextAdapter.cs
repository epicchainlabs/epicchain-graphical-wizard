using System;
using System.Collections.Generic;
using EpicChain.SmartContract;
using StackItem = EpicChain.VM.Types.StackItem;
using EpicChain;
using EpicChain.VM;
using EpicChain.BlockchainToolkit;

namespace EpicChainTraceVisualizer.Neo3
{
    internal partial class DebugApplicationEngine
    {
        private class ExecutionContextAdapter : IExecutionContext
        {
            private readonly ExecutionContext context;

            public ExecutionContextAdapter(ExecutionContext context, IDictionary<UInt160, UInt160> scriptIdMap)
            {
                this.context = context;
                this.ScriptHash = context.GetScriptHash();

                if (scriptIdMap.TryGetValue(this.ScriptHash, out var scriptHash))
                {
                    this.ScriptIdentifier = scriptHash;
                }
                else
                {
                    this.ScriptIdentifier = context.Script.CalculateScriptHash();
                    scriptIdMap[this.ScriptHash] = this.ScriptIdentifier;
                }
            }

            public Instruction? CurrentInstruction => context.CurrentInstruction;

            public int InstructionPointer => context.InstructionPointer;

            public IReadOnlyList<StackItem> EvaluationStack => context.EvaluationStack;

            public IReadOnlyList<StackItem> LocalVariables => Coalese(context.LocalVariables);

            public IReadOnlyList<StackItem> StaticFields => Coalese(context.StaticFields);

            public IReadOnlyList<StackItem> Arguments => Coalese(context.Arguments);

            public Script Script => context.Script;
            public MethodToken[] Tokens => context.GetState<ExecutionContextState>()?.Contract?.Nef?.Tokens
                ?? Array.Empty<MethodToken>();

            public UInt160 ScriptHash { get; }
            public UInt160 ScriptIdentifier { get; }

            static IReadOnlyList<StackItem> Coalese(EpicChain.VM.Slot? slot) => (slot == null) ? Array.Empty<StackItem>() : slot;
        }

    }
}
