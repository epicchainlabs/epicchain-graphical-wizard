using EpicChain.VM;
using System;



namespace EpicChainTraceVisualizer.ModelAdapters
{
    class AdapterBase : StackItem
    {
        public override bool Equals(StackItem other)
        {
            throw new InvalidOperationException();
        }

        public override bool GetBoolean()
        {
            throw new InvalidOperationException();
        }

        public override byte[] GetByteArray()
        {
            throw new InvalidOperationException();
        }
    }
}
