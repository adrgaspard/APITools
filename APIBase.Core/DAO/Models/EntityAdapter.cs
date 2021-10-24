using APIBase.Core.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIBase.Core.DAO.Models
{
    /// <summary>
    /// Represents an adapter for all classes to be adapted to the hierarchy chain of the Entity class.
    /// </summary>
    /// <typeparam name="TAdapted">The type to adapt to the hierarchy chain of the Entity class</typeparam>
    public class EntityAdapter<TAdapted> : Entity where TAdapted : IAdaptedEntity<TAdapted>
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="adapted">The object to be adapted to the hierarchy chain of the Entity class</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="adapted"/> in null</exception>
        public EntityAdapter(TAdapted adapted)
        {
            if (adapted is null)
            {
                throw new ArgumentNullException(nameof(adapted));
            }
            Adapted = adapted;
        }

        /// <summary>
        /// Gets or set the object to be adapted to the hierarchy chain of the Entity class.
        /// </summary>
        [NotMapped]
        public TAdapted Adapted { get; protected init; }

        /// <inheritdoc cref="Entity.Id"/>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id
        {
            get => (Adapted as IGuidResolvable).Id;
            set
            {
                if ((Adapted as IGuidResolvable).Id == Guid.Empty)
                {
                    (Adapted as IGuidWriteable).Id = value;
                }
                else
                {
                    throw new InvalidOperationException("The Id of an Entity can't be edited");
                }
            }
        }

        /// <inheritdoc cref="IValidatable.CanBeDeleted"/>
        public override bool CanBeDeleted()
        {
            return Adapted.CanBeDeleted();
        }

        /// <inheritdoc cref="IValidatable.CanBeSavedOrUpdated"/>
        public override bool CanBeSavedOrUpdated()
        {
            return Adapted.CanBeSavedOrUpdated();
        }
    }
}