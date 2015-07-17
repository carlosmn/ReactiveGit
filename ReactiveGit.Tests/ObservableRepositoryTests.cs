using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Xunit;

namespace ReactiveGit.Tests
{
    public class ObservableRepositoryTests
    {
        [Fact]
        public async Task CanCloneARepository()
        {
            using (var directory = TestDirectory.Create())
            {
                bool completed = false;
                var cloneObserver = new ReplaySubject<Message>();
                cloneObserver.Subscribe((msg) => { }, () => completed = true);
                using (await ObservableRepository.Clone(
                    "https://github.com/shiftkey/rxui-design-guidelines.git",
                    directory.Path,
                    cloneObserver))
                {
                    Assert.NotEmpty(Directory.GetFiles(directory.Path));
                    Assert.True(completed);
                }
            }
        }

        [Fact]
        public void GetProgressFromASyncOperation()
        {
            CredentialsHandler credentials = (url, usernameFromUrl, types) =>
                new UsernamePasswordCredentials
                {
                    Username = "shiftkey-tester",
                    Password = "haha-password"
                };

            var repository = new ObservableRepository(
                @"C:\Users\brendanforster\Documents\GìtHūb\testing-pushspecs",
                credentials);

            var pullObserver = new ReplaySubject<Message>();
            var pushObserver = new ReplaySubject<Message>();

            var pullResult = repository.Pull(pullObserver);

            Assert.NotEqual(MergeStatus.Conflicts, pullResult.Status);

            repository.Push(pushObserver);
        }
    }
}
