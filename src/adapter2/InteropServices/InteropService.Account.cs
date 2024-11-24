using EpicChain.VM;
using EpicChainTraceVisualizer.ModelAdapters;
using System;

namespace EpicChainTraceVisualizer
{
    partial class InteropService
    {
        public void RegisterAccount(Action<string, Func<ExecutionEngine, bool>, int> register)
        {
            register("EpicChain.Account.GetScriptHash", Account_GetScriptHash, 1);
            register("EpicChain.Account.GetVotes", Account_GetVotes, 1);
            register("EpicChain.Account.GetBalance", Account_GetBalance, 1);
            register("EpicChain.Account.IsStandard", Account_IsStandard, 100);

            register("AntShares.Account.GetScriptHash", Account_GetScriptHash, 1);
            register("AntShares.Account.GetVotes", Account_GetVotes, 1);
            register("AntShares.Account.GetBalance", Account_GetBalance, 1);
        }

        private bool Account_GetBalance(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<AccountAdapter>(adapter => adapter.GetBalance(engine));
        }

        private bool Account_GetVotes(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<AccountAdapter>(adapter => adapter.GetVotes(engine));
        }

        private bool Account_GetScriptHash(ExecutionEngine engine)
        {
            return engine.TryAdapterOperation<AccountAdapter>(adapter => adapter.GetScriptHash(engine));
        }

        private bool Account_IsStandard(ExecutionEngine engine)
        {
            throw new NotImplementedException(nameof(Account_IsStandard));
        }
    }
}
