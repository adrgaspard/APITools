using APITools.Core.DAO.Models;

namespace APITools.Core.MVVM
{
    public abstract class ViewModelCRUD<TEntity> : ViewModel where TEntity : class, IGuidResolvable, IValidatable
    {
        // TODO
    }
}