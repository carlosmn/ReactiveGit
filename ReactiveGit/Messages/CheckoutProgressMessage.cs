using System;

namespace ReactiveGit
{
    public class CheckoutProgressMessage : Message
    {
        public string Path { get; private set; }
        public int CompletedSteps { get; private set; }
        public int TotalSteps { get; private set; }

        internal CheckoutProgressMessage(string path, int completedSteps, int totalSteps)
        {
            Path = path;
            CompletedSteps = completedSteps;
            TotalSteps = totalSteps;
        }
    }
}

