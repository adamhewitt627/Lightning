namespace Lightning.Tests.Util
{
    public partial class MethodDependency
    {
        public IService GetService([Inject] IService service) => service;
    }
}
