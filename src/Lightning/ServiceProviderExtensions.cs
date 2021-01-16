using System;

namespace Lightning
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class ServiceProviderExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Registers the provider as the base service provider and returns a delegated wrapper.
        /// The resulting <see cref="IServiceProvider"/> can be used to create scopes and resolve
        /// services.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>Delegated <see cref="IServiceProvider"/>.</returns>
        public static IServiceProvider ToLightning(this IServiceProvider provider)
            => LightningDI.Root = new LightningServiceProvider(provider);
    }
}
