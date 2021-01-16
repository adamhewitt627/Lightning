using System;

namespace Lightning
{
    /// <summary>
    /// An attribute marker that a type of parameter should participate in method generation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Constructor | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }
}
