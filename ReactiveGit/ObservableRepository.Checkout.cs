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
        public void Checkout(
            Branch branch,
            IObserver<CheckoutProgressMessage> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
            };

            _repository.Checkout(branch, options, signature);
        }

        /// <inheritdoc />
        public void Checkout(
            Commit commit,
            IObserver<CheckoutProgressMessage> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
            };

            _repository.Checkout(commit, options, signature);
        }

        /// <inheritdoc />
        public void Checkout(
            string commitOrBranchSpec,
            IObserver<CheckoutProgressMessage> observer)
        {
            var signature = _repository.Config.BuildSignature(DateTimeOffset.Now);

            var options = new CheckoutOptions
            {
                OnCheckoutProgress = ProgressFactory.CreateHandlerForMessage(observer)
            };

            _repository.Checkout(commitOrBranchSpec, options, signature);
        }
    }
}
