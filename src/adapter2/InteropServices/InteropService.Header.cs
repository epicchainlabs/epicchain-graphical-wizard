﻿using EpicChain.VM;
using EpicChainTraceVisualizer.ModelAdapters;
using System;



namespace EpicChainTraceVisualizer
{
    partial class InteropService
    {
        public void RegisterHeader(Action<string, Func<ExecutionEngine, bool>, int> Register)
        {
            Register("EpicChain.Header.GetHash", Header_GetHash, 1);
            Register("EpicChain.Header.GetVersion", Header_GetVersion, 1);
            Register("EpicChain.Header.GetPrevHash", Header_GetPrevHash, 1);
            Register("EpicChain.Header.GetMerkleRoot", Header_GetMerkleRoot, 1);
            Register("EpicChain.Header.GetTimestamp", Header_GetTimestamp, 1);
            Register("EpicChain.Header.GetIndex", Header_GetIndex, 1);
            Register("EpicChain.Header.GetConsensusData", Header_GetConsensusData, 1);
            Register("EpicChain.Header.GetNextConsensus", Header_GetNextConsensus, 1);

            Register("System.Header.GetIndex", Header_GetIndex, 1);
            Register("System.Header.GetHash", Header_GetHash, 1);
            Register("System.Header.GetPrevHash", Header_GetPrevHash, 1);
            Register("System.Header.GetTimestamp", Header_GetTimestamp, 1);

            Register("AntShares.Header.GetHash", Header_GetHash, 1);
            Register("AntShares.Header.GetVersion", Header_GetVersion, 1);
            Register("AntShares.Header.GetPrevHash", Header_GetPrevHash, 1);
            Register("AntShares.Header.GetMerkleRoot", Header_GetMerkleRoot, 1);
            Register("AntShares.Header.GetTimestamp", Header_GetTimestamp, 1);
            Register("AntShares.Header.GetConsensusData", Header_GetConsensusData, 1);
            Register("AntShares.Header.GetNextConsensus", Header_GetNextConsensus, 1);
        }

        private bool Header_GetHash(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetHash(engine, blockchain));
        }

        private bool Header_GetVersion(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetVersion(engine));
        }

        private bool Header_GetPrevHash(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetPrevHash(engine));
        }

        private bool Header_GetMerkleRoot(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetMerkleRoot(engine));
        }

        private bool Header_GetTimestamp(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetTimestamp(engine));
        }

        private bool Header_GetIndex(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetIndex(engine));
        }

        private bool Header_GetConsensusData(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetConsensusData(engine));
        }

        private bool Header_GetNextConsensus(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockHeaderAdapter>(adapter => adapter.GetNextConsensus(engine));
        }
    }
}
