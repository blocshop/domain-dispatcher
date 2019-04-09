using System;

namespace DomainDispatcher.Helper
{
    internal class Disposable : IDisposable
    {
        private readonly Action _disposableAction;

        private Disposable(Action disposableAction)
        {
            _disposableAction = disposableAction;
        }
        public void Dispose()
        {
            _disposableAction();
        }

        public static IDisposable Create(Action disposableAction)
        {
            return new Disposable(disposableAction);
        }
        
    }
}
