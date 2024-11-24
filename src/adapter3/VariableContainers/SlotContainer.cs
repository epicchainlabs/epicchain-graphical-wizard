using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using EpicChain.VM;
using StackItem = EpicChain.VM.Types.StackItem;

namespace EpicChainTraceVisualizer.Neo3
{
    class SlotContainer : IVariableContainer
    {
        private readonly IReadOnlyList<StackItem> slot;
        private readonly string prefix;

        public SlotContainer(string prefix, IReadOnlyList<StackItem>? slot)
        {
            this.slot = slot ?? ImmutableList<StackItem>.Empty;
            this.prefix = prefix;
        }

        public IEnumerable<Variable> Enumerate(IVariableManager manager)
        {
            for (int i = 0; i < slot.Count; i++)
            {
                var v = slot[i].ToVariable(manager, $"{prefix}{i}");
                v.EvaluateName = v.Name;
                yield return v;
            }
        }
    }
}
