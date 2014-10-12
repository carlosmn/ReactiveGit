﻿using System;
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
            IObserver<Tuple<string, int>> observer,
            CredentialsHandler credentials = null)
        {
            var isCancelled = false;
            var options = new CloneOptions
            {
                Checkout = true,
                CredentialsProvider = credentials,
                OnTransferProgress = progress =>
                {
                    // TODO: how should we signal for the "indexing objects" events
                    var p = (100 * progress.ReceivedObjects) / progress.TotalObjects;
                    observer.OnNext(Tuple.Create("", p));
                    return !isCancelled;
                },
                IsBare = false,
                OnCheckoutProgress = ProgressFactory.CreateHandler(observer)
            };

            return Observable.Create<ObservableRepository>(subj =>
            {
                var sub = Observable.Start(() =>
                {
                    var directory = Repository.Clone(sourceUrl, workingDirectory, options);

                    observer.OnNext(Tuple.Create("clone completed", 100));
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
