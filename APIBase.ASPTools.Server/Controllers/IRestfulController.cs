using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBase.ASPTools.Server.Controllers
{
    public interface IRestfulController<TEntity> where TEntity : class
    {
        Task<IActionResult> DeleteOne([FromRoute] Guid id);

        Task<ActionResult<IEnumerable<TEntity>>> GetAll();

        Task<ActionResult<TEntity>> GetOne([FromRoute] Guid id);

        Task<IActionResult> PatchOne([FromRoute] Guid id, [FromBody] JsonPatchDocument<TEntity> patchDocument);

        Task<ActionResult<TEntity>> PostOne([FromBody] TEntity entity);

        Task<IActionResult> PutOne([FromRoute] Guid id, [FromBody] TEntity entity);
    }
}