using System;
using LibGit2Sharp;

namespace ReactiveGit
{
	public class TransferProgressMessage : Message
	{
		public TransferProgress TransferProgress { get; private set; }

		internal TransferProgressMessage(TransferProgress progress)
		{
			this.TransferProgress = progress;
		}
	}
}
