using System;

namespace APITools.Core.Base.DAO.Models
{
    /// <summary>
    /// Represents all the classes that must be adapted to the format of the hierarchical chain of the Entity class.
    /// </summary>
    /// <typeparam name="TAdapted">The type to adapt to the hierarchy chain of the Entity class</typeparam>
    /// <see cref="Entity"/>
    public interface IAdaptedEntity<TAdapted> : IValidatable, IGuidResolvable, IEquatable<IGuidResolvable> where TAdapted : IAdaptedEntity<TAdapted>
    {
        /// <summary>
        /// Gets the computed id for the adapted entity.
        /// </summary>
        /// <remarks>
        /// If the implementation has a mapped "Id" property of another type than Guid, the IGuidResolvable.Id property must be not mapped and must redirect on this one. Also, a unique must be set on this property.
        /// Else, this property must be not mapped and redirects on IGuidResolvable.Id.
        /// In all cases, this property and the IGuidResolvable property must always have the same value.
        /// </remarks>
        Guid ComputedId { get; }

        /// <summary>
        /// Computes and changes the id of the entity if needed.
        /// </summary>
        void ComputesId();
    }
}