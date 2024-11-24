using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using System.Collections.Generic;

namespace EpicChainTraceVisualizer.VariableContainers
{
    public interface IVariableContainer
    {
        IEnumerable<Variable> GetVariables();
    }
}
