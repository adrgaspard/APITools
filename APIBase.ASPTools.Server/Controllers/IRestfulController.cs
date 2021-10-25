using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBase.ASPTools.Server.Controllers
{
    /// <summary>
    /// Represents the methods provided by a REST architecture.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity manipulated by the controller</typeparam>
    public interface IRestfulController<TEntity> where TEntity : class
    {
        /// <summary>
        /// Deletes one item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An empty request result in case of success, an error otherwise</returns>
        Task<IActionResult> DeleteOne([FromRoute] Guid id);

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>A request result that contains all the existing items in case of success, an error otherwise</returns>
        Task<ActionResult<IEnumerable<TEntity>>> GetAll();

        /// <summary>
        /// Gets an existing item.
        /// </summary>
        /// <param name="id">The id of the wanted item</param>
        /// <returns>A request result that contains the item in case of success, an error otherwise</returns>
        Task<ActionResult<TEntity>> GetOne([FromRoute] Guid id);

        /// <summary>
        /// Partially modifies an existing item.
        /// </summary>
        /// <param name="id">The id of the item to be edited</param>
        /// <param name="patchDocument">The modification patch to apply to the item</param>
        /// <returns>An empty request result in case of success, an error otherwise</returns>
        Task<IActionResult> PatchOne([FromRoute] Guid id, [FromBody] JsonPatchDocument<TEntity> patchDocument);

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="entity">The new item</param>
        /// <returns>A request result that contains how to retrieve the created item in case of success, an error otherwise</returns>
        Task<ActionResult<TEntity>> PostOne([FromBody] TEntity entity);

        /// <summary>
        /// Modifies an existing item.
        /// </summary>
        /// <param name="id">The id of the item to be edited</param>
        /// <param name="entity">The new version of the item</param>
        /// <returns>An empty request result in case of success, an error otherwise</returns>
        Task<IActionResult> PutOne([FromRoute] Guid id, [FromBody] TEntity entity);
    }
}