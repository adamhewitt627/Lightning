using Lightning.Tests.Extensions;
using Lightning.Tests.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq.AutoMock;
using System;

namespace Lightning.Tests
{
    [TestClass]
    public class MethodOverloadTests
    {
        public IServiceProvider Provider { get; } = new AutoMocker()
            .AsServiceProvider()
            .ToLightning();

        [TestMethod]
        public void ResolvesServiceFromMethod()
        {
            MethodDependency test = new();
            IService result = test.GetService();
            Assert.AreSame(Provider.GetService<IService>(), result);
        }
    }
}
