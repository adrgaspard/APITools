namespace APITools.Core.Base.MVVM
{
    /// <summary>
    /// Represents all classes that register and own ViewModels (like a Facade).
    /// </summary>
    public interface IViewModelLocator
    {
        /// <summary>
        /// Registers in the right place the ViewModels owned by the object.
        /// </summary>
        void RegisterViewModels();
    }
}