using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        /// <inheritdoc />
        public static IObservable<IObservableRepository> Clone(
            string sourceUrl,
            string workingDirectory,
            IObserver<Message> observer,
            CredentialsHandler credentials = null)
        {
            var isCancelled = false;
            var options = new CloneOptions
            {
                Checkout = true,
                CredentialsProvider = credentials,
                OnTransferProgress = progress =>
                {
                    observer.OnNext(new TransferProgressMessage(progress));
                    return !isCancelled;
                },
                IsBare = false,
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer),
            };

            return Observable.Create<ObservableRepository>(subj =>
            {
                var sub = Observable.Start(() =>
                {
                    var directory = Repository.Clone(sourceUrl, workingDirectory, options);
                    observer.OnCompleted();

                    return new ObservableRepository(directory, credentials);
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
