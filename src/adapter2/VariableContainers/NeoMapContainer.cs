﻿using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using EpicChain.VM;
using System.Collections.Generic;
using System.Linq;

namespace EpicChainTraceVisualizer.VariableContainers
{
    class EpicChainMapContainer : IVariableContainer
    {
        class KvpContainer : IVariableContainer
        {
            private readonly IVariableContainerSession session;
            private readonly StackItem key;
            private readonly StackItem value;

            public KvpContainer(IVariableContainerSession session, StackItem key, StackItem value)
            {
                this.session = session;
                this.key = key;
                this.value = value;
            }

            public IEnumerable<Variable> GetVariables()
            {
                yield return key.GetVariable(session, "key");
                yield return value.GetVariable(session, "value");
            }
        }

        private readonly IVariableContainerSession session;
        private readonly EpicChain.VM.Types.Map map;

        public EpicChainMapContainer(IVariableContainerSession session, EpicChain.VM.Types.Map map)
        {
            this.session = session;
            this.map = map;
        }

        public static Variable Create(IVariableContainerSession session, EpicChain.VM.Types.Map map, string? name)
        {
            var container = new EpicChainMapContainer(session, map);
            var containerID = session.AddVariableContainer(container);

            return new Variable()
            {
                Name = name,
                Type = $"Map[{map.Count}]",
                Value = string.Empty,
                VariablesReference = containerID,
                IndexedVariables = map.Count,
            };
        }

        public IEnumerable<Variable> GetVariables()
        {
            foreach (var (i, kvp) in map.Select((_kvp, _i) => (_i, _kvp)))
            {
                var container = new KvpContainer(session, kvp.Key, kvp.Value);

                yield return new Variable()
                {
                    Name = i.ToString(),
                    Value = string.Empty,
                    VariablesReference = session.AddVariableContainer(container),
                    NamedVariables = 2,
                };
            }
        }
    }
}
