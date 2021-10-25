using System.Globalization;

namespace APIBase.Core.ComponentModel
{
    /// <summary>
    /// Provides a way to apply custom logic to a binding.
    /// </summary>
    /// <typeparam name="TSource">The source type (converted to <typeparamref name="TDestination"/> when calling Convert())</typeparam>
    /// <typeparam name="TDestination">The destination (converted to <typeparamref name="TSource"/> when calling ConvertBack())</typeparam>
    public interface ITypedValueConverter<TSource, TDestination>
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="source">The value to convert</param>
        /// <param name="parameter">The converter parameter to use</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>A converted value</returns>
        TDestination Convert(TSource source, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="source">The value to convert</param>
        /// <param name="parameter">The converter parameter to use</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>A converted value</returns>
        TSource ConvertBack(TDestination source, object parameter, CultureInfo culture);
    }
}