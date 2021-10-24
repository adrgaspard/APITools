using APIBase.Core.DAO.SerializationErrors;

namespace APIBase.Core.DAO.Models
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
        /// <see cref="SetSerializationResultOnSuccess"/>
        /// <see cref="SetSerializationResultOnError"/>
        bool CanBeDeleted();

        /// <summary>
        /// Checks if the object can be saved and calls SetSerializationResultOnSuccess or SetSerializationResultOnError depending on the result.
        /// </summary>
        /// <returns>A value that indicates if the object can be saved</returns>
        /// <see cref="SetSerializationResultOnSuccess"/>
        /// <see cref="SetSerializationResultOnError"/>
        bool CanBeSavedOrUpdated();

        /// <summary>
        /// Set the result of a serialization attempt or a serialization test to an error.
        /// </summary>
        /// <param name="error">The serialization test/attempt result</param>
        void SetSerializationResultOnError(SerializationError error);

        /// <summary>
        /// Set the result of a serialization attempt or a serialization test to a success.
        /// </summary>
        void SetSerializationResultOnSuccess();
    }
}