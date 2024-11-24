using EpicChain.VM;
using EpicChainTraceVisualizer.ModelAdapters;
using System;



namespace EpicChainTraceVisualizer
{
    partial class InteropService
    {
        public void RegisterBlock(Action<string, Func<ExecutionEngine, bool>, int> register)
        {
            register("EpicChain.Block.GetTransactionCount", Block_GetTransactionCount, 1);
            register("EpicChain.Block.GetTransactions", Block_GetTransactions, 1);
            register("EpicChain.Block.GetTransaction", Block_GetTransaction, 1);

            register("System.Block.GetTransactionCount", Block_GetTransactionCount, 1);
            register("System.Block.GetTransactions", Block_GetTransactions, 1);
            register("System.Block.GetTransaction", Block_GetTransaction, 1);

            register("AntShares.Block.GetTransactionCount", Block_GetTransactionCount, 1);
            register("AntShares.Block.GetTransactions", Block_GetTransactions, 1);
            register("AntShares.Block.GetTransaction", Block_GetTransaction, 1);
        }

        private bool Block_GetTransactionCount(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockAdapter>(adapter => adapter.GetTransactionCount(engine));
        }

        private bool Block_GetTransactions(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockAdapter>(adapter => adapter.GetTransactions(engine));
        }

        private bool Block_GetTransaction(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<BlockAdapter>(adapter => adapter.GetTransaction(engine));
        }
    }
}
