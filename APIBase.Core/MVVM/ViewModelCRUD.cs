using APIBase.Core.DAO.Models;

namespace APIBase.Core.MVVM
{
    public abstract class ViewModelCRUD<TEntity> : ViewModel where TEntity : class, IGuidResolvable, IValidatable
    {
        // TODO
    }
}