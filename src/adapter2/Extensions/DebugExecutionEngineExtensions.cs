using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using EpicChainTraceVisualizer.VariableContainers;
using EpicChainFx;

namespace EpicChainTraceVisualizer
{
    static class DebugExecutionEngineExtensions
    {
        public static IVariableContainer GetStorageContainer(this DebugExecutionEngine @this, IVariableContainerSession session, byte[] scriptHash)
            => @this.GetStorageContainer(session, new UInt160(scriptHash));

        public static EvaluateResponse EvaluateStorageExpression(this DebugExecutionEngine @this, IVariableContainerSession session, byte[] scriptHash, EvaluateArguments args)
            => @this.EvaluateStorageExpression(session, new UInt160(scriptHash), args);
    }
}
