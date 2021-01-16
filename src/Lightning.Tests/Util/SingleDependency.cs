namespace Lightning.Tests.Util
{
    public partial class SingleDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public SingleDependency([Inject] IService service)
            : this(string.Empty, service)
        {
        }

        public SingleDependency(string value, [Inject] IService service)
        {
            Value = value ?? throw new System.ArgumentNullException(nameof(value));
            Service = service ?? throw new System.ArgumentNullException(nameof(service));
        }

        public string Value { get; }
        public IService Service { get; }
    }
}
