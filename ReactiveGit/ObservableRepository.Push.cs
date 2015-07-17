using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        /// <inheritdoc />
        public void Push(IObserver<Message> observer)
        {
            var branch = _repository.Head;

            var isCancelled = false;
            var options = new PushOptions
            {
                CredentialsProvider = _credentialsHandler,
                OnPushTransferProgress = (current, total, bytes) =>
                {
                    observer.OnNext(new PushTransferProgressMessage(current, total, bytes));
                    return !isCancelled;
                }
            };

            _repository.Network.Push(branch, options);
        }
    }
}
