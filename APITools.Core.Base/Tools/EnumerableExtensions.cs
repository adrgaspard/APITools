using System;
using System.Collections.Generic;

namespace APITools.Core.Base.Tools
{
    /// <summary>
    /// Represents a set of methods to facilitate manipulations on collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Adds multiple items to a source collection.
        /// </summary>
        /// <typeparam name="TItem">Type of items</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="toAdd">Items to add to the source collection</param>
        public static void AddRange<TItem>(this ICollection<TItem> source, IEnumerable<TItem> toAdd)
        {
            foreach (TItem item in toAdd)
            {
                source.Add(item);
            }
        }

        /// <summary>
        /// Checks if a source collection does not have the same element twice (the equality protocol, not the identity protocol, is used for the check).
        /// </summary>
        /// <typeparam name="TItem">Type of items</typeparam>
        /// <param name="source">The source collection</param>
        /// <returns></returns>
        public static bool ContainsOnlyUniqueElements<TItem>(this ICollection<TItem> source)
        {
            List<TItem> alreadyFound = new();
            foreach (TItem item in source)
            {
                if (alreadyFound.Contains(item))
                {
                    return false;
                }
                alreadyFound.Add(item);
            }
            return true;
        }

        /// <summary>
        /// Removes multiple items from a source collection.
        /// </summary>
        /// <typeparam name="TItem">Type of items</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="toRemove">Items to remove from the source collection</param>
        public static void RemoveRange<TItem>(this ICollection<TItem> source, IEnumerable<TItem> toRemove)
        {
            foreach (TItem item in toRemove)
            {
                source.Remove(item);
            }
        }

        /// <summary>
        /// Removes multiple items from a source collection based on a predicate.
        /// </summary>
        /// <typeparam name="TItem">Type of items</typeparam>
        /// <param name="source">The source collection</param>
        /// <param name="predicate">Predicate that indicates which items should be removed from the source collection</param>
        public static void RemoveRange<TItem>(this IList<TItem> source, Predicate<TItem> predicate)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                if (predicate(source[i]))
                {
                    source.RemoveAt(i);
                }
            }
        }
    }
}