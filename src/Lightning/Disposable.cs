using System;

namespace Lightning
{
    internal abstract class Disposable : IDisposable
    {
        protected abstract void Dispose(bool disposing);

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Disposable()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}