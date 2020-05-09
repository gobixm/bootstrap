using System.Threading;

namespace Gobi.Bootstrap
{
    internal sealed class BootstrapState : IBootstrapState
    {
        private readonly ReaderWriterLockSlim _progressLock = new ReaderWriterLockSlim();
        private string _progress = string.Empty;

        public string Progress
        {
            get
            {
                _progressLock.EnterReadLock();
                try
                {
                    return _progress;
                }
                finally
                {
                    _progressLock.ExitReadLock();
                }
            }
            set
            {
                _progressLock.EnterWriteLock();
                try
                {
                    _progress = value;
                }
                finally
                {
                    _progressLock.ExitWriteLock();
                }
            }
        }

        public void Dispose()
        {
            _progressLock?.Dispose();
        }
    }
}