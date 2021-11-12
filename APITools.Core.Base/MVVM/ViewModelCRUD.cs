using APITools.Core.Base.DAO.Models;

namespace APITools.Core.Base.MVVM
{
    public abstract class ViewModelCRUD<TEntity> : ViewModel where TEntity : class, IGuidResolvable, IValidatable
    {
        // TODO
    }
}