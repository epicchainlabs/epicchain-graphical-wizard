﻿using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using System.Collections.Generic;

namespace EpicChainTraceVisualizer.VariableContainers
{
    class EpicChainArrayContainer : IVariableContainer
    {
        private readonly IVariableContainerSession session;
        private readonly EpicChain.VM.Types.Array array;
        private readonly string? name;

        private EpicChainArrayContainer(IVariableContainerSession session, EpicChain.VM.Types.Array array, string name)
        {
            this.session = session;
            this.array = array;
            this.name = name;
        }

        public static Variable Create(IVariableContainerSession session, EpicChain.VM.Types.Array array, string name)
        {
            var container = new EpicChainArrayContainer(session, array, name);
            var containerID = session.AddVariableContainer(container);
            var typeName = array is EpicChain.VM.Types.Struct
                ? "Struct" : "Array";
            return new Variable()
            {
                Name = name,
                Type = $"{typeName}[{array.Count}]",
                Value = string.Empty,
                VariablesReference = containerID,
                IndexedVariables = array.Count,
            };
        }

        public IEnumerable<Variable> GetVariables()
        {
            for (int i = 0; i < array.Count; i++)
            {
                var variable = array[i].GetVariable(session, i.ToString());
                variable.EvaluateName = $"{name}[{i}]";
                variable.Value = variable.Type;
                yield return variable;
            }
        }
    }
}
