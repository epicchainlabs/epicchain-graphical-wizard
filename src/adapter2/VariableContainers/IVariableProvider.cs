using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;

namespace EpicChainTraceVisualizer.VariableContainers
{
    public interface IVariableProvider
    {
        Variable GetVariable(IVariableContainerSession session, string name);
    }
}
