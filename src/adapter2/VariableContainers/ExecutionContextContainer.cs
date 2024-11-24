using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using EpicChain.VM;
using EpicChainTraceVisualizer.Models;
using System.Collections.Generic;
using System.Linq;

namespace EpicChainTraceVisualizer.VariableContainers
{
    class ExecutionContextContainer : IVariableContainer
    {
        private readonly IVariableContainerSession session;
        private readonly ExecutionContext context;
        private readonly DebugInfo.Method? method;

        public ExecutionContextContainer(IVariableContainerSession session, ExecutionContext context, Contract contract)
            : this(session, context, contract.GetMethod(context))
        {
        }

        public ExecutionContextContainer(IVariableContainerSession session, ExecutionContext context, DebugInfo.Method? method)
        {
            this.session = session;
            this.context = context;
            this.method = method;
        }

        public IEnumerable<Variable> GetVariables()
        {
            if (context.AltStack.Count > 0)
            {
                var variables = (EpicChain.VM.Types.Array)context.AltStack.Peek(0);

                for (int i = 0; i < variables.Count; i++)
                {
                    var parameter = method?.GetLocals().ElementAtOrDefault(i);
                    var name = parameter?.name ?? $"<variable {i}>";
                    var variable = variables[i].GetVariable(session, name, parameter?.type);
                    if (!string.IsNullOrEmpty(parameter?.name))
                    {
                        variable.EvaluateName = name;
                    }
                    yield return variable;
                }
            }
        }
    }
}
