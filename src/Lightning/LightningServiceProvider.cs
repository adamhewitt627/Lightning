using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lightning
{
    internal class LightningServiceProvider : IServiceProvider
    {
        public LightningServiceProvider(IServiceProvider provider)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IServiceProvider Provider { get; }

        public object GetService(Type serviceType)
        {
            if (ReferenceEquals(serviceType, typeof(IServiceScopeFactory))
                && Provider.GetService<IServiceScopeFactory>() is { } factory)
            {
                return new LightningServiceScopeFactory(factory);
            }

            return Provider.GetRequiredService(serviceType);
        }
    }
}
