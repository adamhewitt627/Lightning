using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lightning
{
    internal class LightningServiceScopeFactory : IServiceScopeFactory
    {
        public LightningServiceScopeFactory(IServiceScopeFactory factory)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IServiceScopeFactory Factory { get; }

        /// <inheritdoc />
        public IServiceScope CreateScope() => new LightningServiceScope(Factory.CreateScope());

        private class LightningServiceScope : Disposable, IServiceScope
        {
            private readonly IServiceScope _serviceScope;
            private bool _disposed;

            public LightningServiceScope(IServiceScope serviceScope)
            {
                _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
                ServiceProvider = new LightningServiceProvider(_serviceScope.ServiceProvider);
            }

            public IServiceProvider ServiceProvider { get; }

            protected override void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    ServiceProvider.Pop();
                    _serviceScope.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
