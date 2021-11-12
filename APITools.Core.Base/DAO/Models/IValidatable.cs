namespace APITools.Core.Base.DAO.Models
{
    /// <summary>
    /// Represents any object that can be validated for a potential add/update/delete in a serialization context.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Checks if the object can be deleted and calls SetSerializationResultOnSuccess or SetSerializationResultOnError depending on the result.
        /// </summary>
        /// <returns>A value that indicates if the object can be deleted</returns>
        SerializationResult CanBeDeleted();

        /// <summary>
        /// Checks if the object can be saved or updated and calls SetSerializationResultOnSuccess or SetSerializationResultOnError depending on the result.
        /// </summary>
        /// <returns>A value that indicates if the object can be saved or updated</returns>
        SerializationResult CanBeSavedOrUpdated();
    }
}