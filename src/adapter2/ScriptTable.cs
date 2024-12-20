﻿using EpicChainTraceVisualizer.Models;
using EpicChainFx;
using System;
using System.Collections.Generic;

namespace EpicChainTraceVisualizer
{
    class ScriptTable : EpicChain.VM.IScriptTable
    {
        private readonly Dictionary<UInt160, byte[]> scripts = new Dictionary<UInt160, byte[]>();

        public void Add(byte[] script)
            => scripts.Add(Crypto.HashScript(script), script);

        public byte[]? GetScript(byte[] scriptHash)
            => GetScript(new UInt160(scriptHash));

        public byte[]? GetScript(in UInt160 scriptHash)
            => scripts.TryGetValue(scriptHash, out var script)
                ? script : null;
    }
}
