using System;

namespace APITools.Core.Ioc
{
    /// <summary>
    /// When used with the Ioc container, specifies which constructor, should be used to instantiate when GetInstance is called. If there is only one constructor in the class, this attribute is not needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public class MainConstructorAttribute : Attribute
    {
    }
}