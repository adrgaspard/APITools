namespace APITools.Core.Base.Tools
{
    /// <summary>
    /// Represents a set of methods to facilitate certain mathematical calculations.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Checks if an integer is a power of 2.
        /// </summary>
        /// <param name="number">Number to be checked</param>
        /// <returns>A value that indicates whether the number is a power of 2</returns>
        public static bool IsPowerOfTwo(int number)
        {
            return (number & number - 1) == 0 && number > 0;
        }

        /// <inheritdoc cref="IsPowerOfTwo(int)"/>
        public static bool IsPowerOfTwo(long number)
        {
            return (number & number - 1) == 0 && number > 0;
        }
    }
}