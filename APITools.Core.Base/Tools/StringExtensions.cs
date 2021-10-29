using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace APITools.Core.Tools
{
    /// <summary>
    /// Represents a set of methods to facilitate manipulation and calculations on strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The pattern describing all strings composed uniquely of at least one white-space char.
        /// </summary>
        public static readonly Regex WhiteSpacesPattern = new(@"\s+");

        /// <summary>
        /// Checks if a string includes another string.
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="pattern">The string potentially contained in the source string</param>
        /// <param name="comparison">Comparison method for both strings</param>
        /// <returns>A value that indicates whether the source string contains the pattern string</returns>
        public static bool Contains(this string source, string pattern, StringComparison comparison)
        {
            return source?.IndexOf(pattern, comparison) >= 0;
        }

        /// <summary>
        /// Counts the number of occurrences of a pattern in a string.
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="pattern">The pattern to be identified in the source string</param>
        /// <param name="distinctsOccurences">Indicates whether the hits found must be in distinct locations (i.e. there cannot be two matching hits for a given index in the source string)</param>
        /// <returns></returns>
        public static int CountOccurencesOf(this string source, string pattern, bool distinctsOccurences = true)
        {
            int count = 0;
            int i = 0;
            while ((i = source.IndexOf(pattern, i)) != -1)
            {
                if (distinctsOccurences)
                {
                    i += pattern.Length;
                }
                else
                {
                    i++;
                }
                count++;
            }
            return count;
        }

        /// <summary>
        /// Gets the Levenshtein distance between two strings.
        /// This represents the minimum number of unit operations (addition or deletion of a character at a given index) that must be performed to move from one string to another.
        /// </summary>
        /// <param name="source">The first string for the calculation</param>
        /// <param name="target">The second string for the calculation</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Occurs when the <paramref name="source"/> or the <paramref name="target"/> is null</exception>
        public static int GetLevenshteinDistanceFrom(this string source, string target)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            int sourceCharCount = source.Length;
            int targetCharCount = target.Length;
            if (sourceCharCount == 0)
            {
                return targetCharCount;
            }
            if (targetCharCount == 0)
            {
                return sourceCharCount;
            }
            if (source == target)
            {
                return 0;
            }
            int[,] distance = new int[sourceCharCount + 1, targetCharCount + 1];
            for (int i = 0; i <= sourceCharCount; distance[i, 0] = i++)
            {
                ;
            }

            for (int i = 0; i <= targetCharCount; distance[0, i] = i++)
            {
                ;
            }

            for (int i = 1; i <= sourceCharCount; i++)
            {
                for (int j = 1; j <= targetCharCount; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }
            return distance[sourceCharCount, targetCharCount];
        }

        /// <summary>
        /// Gets the similarity between two strings.
        /// </summary>
        /// <param name="source">The first string for the calculation</param>
        /// <param name="target">The second string for the calculation</param>
        /// <returns>A number between 0 and 1 (0 being the absolute difference between the 2 strings and 1 being the equality between them)</returns>
        public static double GetSimilarityWith(this string source, string target)
        {
            return Math.Max(1, Math.Min(0, 1d - (source.GetLevenshteinDistanceFrom(target) / (double)Math.Max(source.Length, target.Length))));
        }

        /// <summary>
        /// Gets a new version of a string in which only the first character is capitalized (the others are lowercase).
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The new version of the string</returns>
        public static string ToFirstLetterUpper(this string source)
        {
            return string.IsNullOrWhiteSpace(source) ? source : source.Substring(0, 1).ToUpper() + source[1..].ToLower();
        }

        /// <summary>
        /// Generates a secure string for a given string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="makeReadOnly">Indicates whether the resulting secure string should be read-only or not</param>
        /// <returns>The generated secure string</returns>
        public static SecureString ToSecureString(this string source, bool makeReadOnly)
        {
            SecureString result = new();
            foreach (char c in source)
            {
                result.AppendChar(c);
            }
            if (makeReadOnly)
            {
                result.MakeReadOnly();
            }
            return result;
        }

        /// <summary>
        /// Generates a readable string from a given secure string.
        /// </summary>
        /// <param name="source">The source secure string</param>
        /// <returns>The generated string</returns>
        public static string ToUnsecureString(this SecureString source)
        {
            if (source is null)
            {
                return null;
            }
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(source);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Gets a non-accented version of a given string.
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The non-accented version of the source string</returns>
        public static string WithoutAccents(this string source)
        {
            StringBuilder result = new();
            foreach (char c in source.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets a version without whitespaces of a given string.
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The version without whitespaces of the source string</returns>
        public static string WithoutWhitespaces(this string source)
        {
            return WhiteSpacesPattern.Replace(source, string.Empty);
        }
    }
}