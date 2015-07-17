using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using LibGit2Sharp;

namespace ReactiveGit
{
    public partial class ObservableRepository
    {
        /// <inheritdoc />
        public IObservable<Unit> Checkout(
            Branch branch,
            IObserver<CheckoutProgressMessage> observer)
        {
           var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(branch, options, signature);
                observer.OnCompleted();
            }, Scheduler.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> Checkout(
            Commit commit,
            IObserver<CheckoutProgressMessage> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(commit, options, signature);
                observer.OnCompleted();
            }, Scheduler.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> Checkout(
            string commitOrBranchSpec,
            IObserver<CheckoutProgressMessage> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
            };

            return Observable.Start(() =>
            {
                _repository.Checkout(commitOrBranchSpec, options, signature);
                observer.OnCompleted();
            }, Scheduler.Default);
        }
    }
}
