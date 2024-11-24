using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using System.Collections.Generic;

namespace EpicChainTraceVisualizer.Neo3
{
    public interface IVariableContainer
    {
        IEnumerable<Variable> Enumerate(IVariableManager manager);
    }
}
