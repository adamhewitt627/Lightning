using Lightning.Tests.Extensions;
using Lightning.Tests.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq.AutoMock;
using System;

namespace Lightning.Tests
{
    [TestClass]
    public class SimpleConstructorTests
    {
        public IServiceProvider Provider { get; } = new AutoMocker()
            .AsServiceProvider()
            .ToLightning();

        [TestMethod]
        public void Resolves_from_empty_constructor()
        {
            SingleDependency payload = new();
            Assert.AreSame(Provider.GetService<IService>(), payload.Service);
        }

        [TestMethod]
        public void Resolves_from_empty_record_constructor()
        {
            RecordWithDependencies payload = new();
            Assert.AreSame(Provider.GetService<IService>(), payload.Service);
        }

        [DataTestMethod]
        [DataRow("Just another day...")]
        [DataRow("...may the force be with you.")]
        public void Resolves_from_single_parameter_constructor(string value)
        {
            SingleDependency payload = new(value);
            Assert.AreSame(value, payload.Value);
            Assert.AreSame(Provider.GetService<IService>(), payload.Service);
        }

    }
}
