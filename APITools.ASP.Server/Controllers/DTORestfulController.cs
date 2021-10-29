using APITools.Core.ComponentModel;
using APITools.Core.DAO.Models;
using APITools.Core.Ioc;
using APITools.Core.Server.DAO.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITools.ASP.Server.Controllers
{
    /// <summary>
    /// Represents the base class for all restful controllers that use DTO to protect some data in the entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities manipulated</typeparam>
    /// <typeparam name="TMementoDTO">The type of the mementos used to save and restore certain states on entities, also used as a <typeparamref name="TEntity"/> DTO</typeparam>
    public abstract class DTORestfulController<TEntity, TMementoDTO> : ControllerBase, IRestfulController<TMementoDTO> where TEntity : class, IGuidResolvable, IValidatable, IOriginator<TMementoDTO>, new() where TMementoDTO : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected DTORestfulController() : this(true)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="automaticallySetRepositoryWithIoc">Whether the controller repository is to be automatically searched with the service locator or not</param>
        protected DTORestfulController(bool automaticallySetRepositoryWithIoc)
        {
            if (automaticallySetRepositoryWithIoc)
            {
                Repository = ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }
        }

        /// <summary>
        /// Gets or sets the repository used by the controller.
        /// </summary>
        protected IRepository<TEntity> Repository { get; init; }

        /// <inheritdoc cref="IRestfulController{TEntity}.DeleteOne(Guid)"/>
        public virtual async Task<IActionResult> DeleteOne([FromRoute] Guid id)
        {
            TEntity entity = await Repository.FindOneAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            SerializationResult result = await Repository.CanDeleteAsync(entity);
            if (!result.IsOk)
            {
                return BadRequest(result);
            }
            await Repository.DeleteAsync(entity);
            return NoContent();
        }

        /// <inheritdoc cref="IRestfulController{TEntity}.GetAll"/>
        public virtual async Task<ActionResult<IEnumerable<TMementoDTO>>> GetAll()
        {
            IEnumerable<TEntity> entities = await Repository.FindAllAsync();
            return new ActionResult<IEnumerable<TMementoDTO>>(entities.Select(entity => entity.CreateMemento()));
        }

        /// <inheritdoc cref="IRestfulController{TEntity}.GetOne(Guid)"/>
        public virtual async Task<ActionResult<TMementoDTO>> GetOne([FromRoute] Guid id)
        {
            TEntity entity = await Repository.FindOneAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            return entity.CreateMemento();
        }

        /// <inheritdoc cref="IRestfulController{TEntity}.PatchOne(Guid, JsonPatchDocument{TEntity})"/>
        public virtual async Task<IActionResult> PatchOne([FromRoute] Guid id, [FromBody] JsonPatchDocument<TMementoDTO> patchDocument)
        {
            if (patchDocument is null)
            {
                return BadRequest(ModelState);
            }
            TEntity entity = await Repository.FindOneAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            TMementoDTO oldMemento = entity.CreateMemento();
            TMementoDTO newMemento = entity.CreateMemento();
            patchDocument.ApplyTo(newMemento);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            entity.InstallMemento(newMemento);
            SerializationResult result = await Repository.CanUpdateAsync(entity);
            if (!result.IsOk)
            {
                entity.InstallMemento(oldMemento);
                return BadRequest(result);
            }
            await Repository.SaveUpdateAsync(entity);
            return new ObjectResult(entity);
        }

        /// <inheritdoc cref="IRestfulController{TEntity}.PostOne(TEntity)"/>
        public virtual async Task<ActionResult<TMementoDTO>> PostOne([FromBody] TMementoDTO entity)
        {
            if (entity is null)
            {
                return BadRequest(ModelState);
            }
            TEntity newEntity = new();
            newEntity.InstallMemento(entity);
            SerializationResult result = await Repository.CanSaveAsync(newEntity);
            if (!result.IsOk)
            {
                return BadRequest(result);
            }
            await Repository.AddAsync(newEntity);
            return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
        }

        /// <inheritdoc cref="IRestfulController{TEntity}.PutOne(Guid, TEntity)"/>
        public virtual async Task<IActionResult> PutOne([FromRoute] Guid id, [FromBody] TMementoDTO entity)
        {
            if (entity is null)
            {
                return BadRequest(ModelState);
            }
            TEntity trackedEntity = await Repository.FindOneAsync(id);
            if (trackedEntity is null)
            {
                return NotFound();
            }
            TMementoDTO memento = trackedEntity.CreateMemento();
            trackedEntity.InstallMemento(entity);
            SerializationResult result = await Repository.CanUpdateAsync(trackedEntity);
            if (!result.IsOk)
            {
                trackedEntity.InstallMemento(memento);
                return BadRequest(result);
            }
            await Repository.SaveUpdateAsync(trackedEntity);
            return NoContent();
        }
    }
}