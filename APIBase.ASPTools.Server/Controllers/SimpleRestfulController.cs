using APIBase.Core.ComponentModel;
using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.Repositories;
using APIBase.Core.Ioc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBase.ASPTools.Server.Controllers
{
    public abstract class SimpleRestfulController<TEntity, TMemento> : ControllerBase, IRestfulController<TEntity> where TEntity : class, IGuidResolvable, IValidatable, IOriginator<TMemento>
    {
        protected SimpleRestfulController() : this(true)
        {
        }

        protected SimpleRestfulController(bool automaticallySetRepositoryWithIoc)
        {
            if (automaticallySetRepositoryWithIoc)
            {
                Repository = ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }
        }

        protected IRepository<TEntity> Repository { get; init; }

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

        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return new ActionResult<IEnumerable<TEntity>>(await Repository.FindAllAsync());
        }

        public virtual async Task<ActionResult<TEntity>> GetOne([FromRoute] Guid id)
        {
            TEntity entity = await Repository.FindOneAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            return entity;
        }

        public virtual async Task<IActionResult> PatchOne([FromRoute] Guid id, [FromBody] JsonPatchDocument<TEntity> patchDocument)
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
            TMemento memento = entity.CreateMemento();
            patchDocument.ApplyTo(entity);
            if (!ModelState.IsValid)
            {
                entity.InstallMemento(memento);
                return BadRequest(ModelState);
            }
            SerializationResult result = await Repository.CanUpdateAsync(entity);
            if (!result.IsOk)
            {
                entity.InstallMemento(memento);
                return BadRequest(result);
            }
            await Repository.SaveUpdateAsync(entity);
            return new ObjectResult(entity);
        }

        public virtual async Task<ActionResult<TEntity>> PostOne([FromBody] TEntity entity)
        {
            if (entity is null)
            {
                return BadRequest(ModelState);
            }
            SerializationResult result = await Repository.CanSaveAsync(entity);
            if (!result.IsOk)
            {
                return BadRequest(result);
            }
            await Repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
        }

        public virtual async Task<IActionResult> PutOne([FromRoute] Guid id, [FromBody] TEntity entity)
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
            TMemento memento = trackedEntity.CreateMemento();
            trackedEntity.InstallMemento(entity.CreateMemento());
            SerializationResult result = await Repository.CanUpdateAsync(entity);
            if (!result.IsOk)
            {
                trackedEntity.InstallMemento(memento);
                return BadRequest(result);
            }
            await Repository.SaveUpdateAsync(entity);
            return NoContent();
        }
    }
}