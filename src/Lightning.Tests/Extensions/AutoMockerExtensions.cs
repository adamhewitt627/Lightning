using Moq.AutoMock;
using System;

namespace Lightning.Tests.Extensions
{
    public static class AutoMockerExtensions
    {
        public static IServiceProvider AsServiceProvider(this AutoMocker mocker)
        {
            return new AutoMockerServiceProvider(mocker);
        }

        private class AutoMockerServiceProvider : IServiceProvider
        {
            private readonly AutoMocker _mocker;

            public AutoMockerServiceProvider(AutoMocker mocker)
            {
                _mocker = mocker ?? throw new ArgumentNullException(nameof(mocker));
            }

            public object? GetService(Type serviceType) => _mocker.Get(serviceType);
        }
    }
}
