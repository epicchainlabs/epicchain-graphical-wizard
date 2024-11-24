using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using Neo;

namespace NeoDebug.Neo3
{
    using NeoMap = EpicChain.VM.Types.Map;

    class NeoMapContainer : IVariableContainer
    {
        private readonly NeoMap map;

        public NeoMapContainer(NeoMap map)
        {
            this.map = map;
        }

        public static Variable Create(IVariableManager manager, NeoMap map, string name)
        {
            var container = new NeoMapContainer(map);
            return new Variable()
            {
                Name = name,
                Value = $"Map[{map.Count}]",
                VariablesReference = manager.Add(container),
                NamedVariables = map.Count
            };
        }

        public IEnumerable<Variable> Enumerate(IVariableManager manager)
        {
            foreach (var key in map.Keys)
            {
                var keyString = key switch
                {
                    EpicChain.VM.Types.Boolean @bool => @bool.GetBoolean().ToString(),
                    EpicChain.VM.Types.ByteString byteString => byteString.GetSpan().ToHexString(),
                    EpicChain.VM.Types.Integer @int => @int.GetInteger().ToString(),
                    _ => throw new NotImplementedException($"Unknown primitive type {key.GetType()}"),
                };

                yield return map[key].ToVariable(manager, keyString);
            }
        }
    }
}
