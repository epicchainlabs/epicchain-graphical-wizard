using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using System.Collections.Generic;

namespace EpicChainTraceVisualizer.EpicChain
{
    public interface IVariableContainer
    {
        IEnumerable<Variable> Enumerate(IVariableManager manager);
    }
}
