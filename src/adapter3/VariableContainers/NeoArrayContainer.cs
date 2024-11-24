using System.Collections.Generic;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;

namespace EpicChainTraceVisualizer.EpicChain
{
    using epicchainArray = EpicChain.VM.Types.Array;
    using NeoStruct = EpicChain.VM.Types.Struct;

    class EpicChainArrayContainer : IVariableContainer
    {
        private readonly epicchainArray array;

        public EpicChainArrayContainer(epicchainArray array)
        {
            this.array = array;
        }

        public static Variable Create(IVariableManager manager, epicchainArray array, string name)
        {
            var typeName = array is NeoStruct ? "Struct" : "Array";
            var container = new EpicChainArrayContainer(array);
            return new Variable()
            {
                Name = name,
                Value = $"{typeName}[{array.Count}]",
                VariablesReference = manager.Add(container),
                IndexedVariables = array.Count,
            };
        }

        public IEnumerable<Variable> Enumerate(IVariableManager manager)
        {
            for (int i = 0; i < array.Count; i++)
            {
                yield return array[i].ToVariable(manager, $"{i}");
            }
        }
    }
}
