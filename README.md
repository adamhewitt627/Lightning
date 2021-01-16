# Lightning
This is a proof-of-concept to generated static DI requests for class that have dependencies but are `new`-ed up in code. For example:

```cs
//Given a class such as:
public partial class SingleDependency
{
    public SingleDependency([Inject] IService service)
    {
        _Service = service;
    }
}


//Allows IService to be resolved from the DI container with simply:
_ = new SingleDependency();
```

This is by no means the first tool of its kind, [@Keboo/AutoDI](https://github.com/keboo/autodi) is a good one. This one leverages C#9 generators and makes no effort to be a DI container, it simply wraps an existing one.