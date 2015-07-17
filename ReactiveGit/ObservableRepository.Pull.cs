using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        /// <inheritdoc />
        public IObservable<MergeResult> Pull(
            IObserver<Message> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);
            var isCancelled = false;

            var options = new PullOptions
            {
                FetchOptions = new FetchOptions
                {
                    TagFetchMode = TagFetchMode.None,
                    CredentialsProvider = _credentialsHandler,
                    OnTransferProgress = progress =>
                    {
                        observer.OnNext(new TransferProgressMessage(progress));            
                        return !isCancelled;
                    }
                },
                MergeOptions = new MergeOptions
                {
                    OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
                }
            };

            return Observable.Create<MergeResult>(subj =>
            {
                var sub = Observable.Start(() =>
                {
                    var result = _repository.Network.Pull(signature, options);

                    observer.OnCompleted();

                    return result;
                }, Scheduler.Default).Subscribe(subj);

                return new CompositeDisposable(
                    sub,
                    Disposable.Create(() =>
                    {
                        isCancelled = true;
                        observer.OnCompleted();
                    }));
            });
        }
    }
}
