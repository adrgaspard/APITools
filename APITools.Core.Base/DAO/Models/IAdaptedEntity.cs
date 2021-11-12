using System;

namespace APITools.Core.Base.DAO.Models
{
    /// <summary>
    /// Represents all the classes that must be adapted to the format of the hierarchical chain of the Entity class.
    /// </summary>
    /// <typeparam name="TAdapted">The type to adapt to the hierarchy chain of the Entity class</typeparam>
    /// <see cref="Entity"/>
    public interface IAdaptedEntity<TAdapted> : IValidatable, IGuidResolvable, IGuidWriteable, IEquatable<IGuidResolvable> where TAdapted : IAdaptedEntity<TAdapted>
    {
        /// <summary>
        /// Gets or sets the adapter to make the object compatible with the hierarchical chain of the Entity class.
        /// </summary>
        /// <see cref="Entity"/>
        EntityAdapter<TAdapted> Adapter { get; }
    }
}