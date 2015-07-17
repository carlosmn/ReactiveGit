using System;

namespace ReactiveGit
{
    public class PushTransferProgressMessage : Message
    {
        public int Current { get; private set; }
        public int Total { get; private set; }
        public long Bytes { get; private set; }
                
        internal PushTransferProgressMessage(int current, int total, long bytes)
        {
            Current = current;
            Total = total;
            Bytes = bytes;
        }
    }
}

