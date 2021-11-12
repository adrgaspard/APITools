namespace APITools.Core.Base.ComponentModel
{
    /// <summary>
    /// Represents a way to without violating encapsulation, capture and externalize an object's internal state so that the object can be restored to this state later.
    /// </summary>
    /// <typeparam name="TMemento">The type that stores internal state of the originator object</typeparam>
    public interface IOriginator<TMemento>
    {
        /// <summary>
        /// Creates a memento based on the actual state of the object.
        /// </summary>
        /// <returns>The new memento</returns>
        TMemento CreateMemento();

        /// <summary>
        /// Installs the memento saved state on the object.
        /// </summary>
        /// <param name="memento">The memento to install</param>
        void InstallMemento(TMemento memento);
    }
}