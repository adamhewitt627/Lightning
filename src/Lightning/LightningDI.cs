using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Lightning
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class LightningDI
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        internal static IServiceProvider? Root { get; set; }
        private static AsyncLocal<Stack<IServiceProvider>?> Providers { get; } = new();

        internal static void Push(this IServiceProvider provider)
        {
            var stack = Providers.Value ??= new();
            stack.Push(provider);
        }

        internal static void Pop(this IServiceProvider provider)
        {
            var stack = Providers.Value ??= new();
            if (stack is null || !stack.TryPeek(out var top) || !ReferenceEquals(provider, top))
                throw new ArgumentException($"'{nameof(provider)}' disposed out of order.", nameof(provider));

            stack.Pop();
        }

        /// <summary>
        /// This is generally an implementation detail, and isn't meant o be called by the user. However,
        /// because it is used by generated C#, it must also be public. This resolves the requested service from the 
        /// current container or scope.
        /// </summary>
        /// <typeparam name="T">The type of dependency to resolve from the IoC container.</typeparam>
        /// <returns>The implementation of <typeparamref name="T"/> resolved from the container.</returns>
        public static T Resolve<T>()
            where T : notnull
        {
            Providers.Value ??= new();

            if (!Providers.Value.TryPeek(out var provider))
                provider = Root;

            if (provider is null)
                throw new InvalidOperationException($"'{nameof(LightningDI)}' has not been initialized.");

            return provider.GetRequiredService<T>();
        }
    }
}
