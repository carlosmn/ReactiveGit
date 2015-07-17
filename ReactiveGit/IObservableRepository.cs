using System;
using System.Reactive;
using LibGit2Sharp;

namespace ReactiveGit
{
    /// <summary>
    /// Abstraction for performing asynchronous operations against a LibGit2Sharp repository
    /// </summary>
    public interface IObservableRepository : IDisposable
    {
        /// <summary>
        /// Push the changes from this repository to the remote repository
        /// </summary>
        /// <param name="observer">An observer to report progress</param>
        void Push(IObserver<Message> observer);

        /// <summary>
        /// Pull changes from the remote repository into this repository
        /// </summary>
        /// <param name="observer">An observer to report progress</param>
        /// <returns>The result of the merge</returns>
        MergeResult Pull(IObserver<Message> observer);

        /// <summary>
        /// Checkout a specific commit for this repository
        /// </summary>
        /// <param name="commit">The desired commit</param>
        /// <param name="observer">An observer to report progress</param>
        void Checkout(Commit commit, IObserver<CheckoutProgressMessage> observer);

        /// <summary>
        /// Checkout a specific branch for this repository
        /// </summary>
        /// <param name="branch">The desired branch</param>
        /// <param name="observer">An observer to report progress</param>
        void Checkout(Branch branch, IObserver<CheckoutProgressMessage> observer);

        /// <summary>
        /// Checkout a specific commitish for this repository
        /// </summary>
        /// <param name="commitOrBranchSpec">The desired commit</param>
        /// <param name="observer">An observer to report progress</param>
        void Checkout(string commitOrBranchSpec, IObserver<CheckoutProgressMessage> observer);

        /// <summary>
        /// Access to the underlying LibGit2Sharp repository
        /// </summary>
        IRepository Inner { get; }
    }
}
