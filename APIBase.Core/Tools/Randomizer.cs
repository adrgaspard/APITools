using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIBase.Core.Tools
{
    /// <summary>
    /// Represents a set of methods to better manage the use of the Random class.
    /// </summary>
    public abstract class Randomizer
    {
        /// <summary>
        /// The list of default characters for generating strings.
        /// </summary>
        /// <see cref="NextString(int, IEnumerable{char})"/>
        public const string BasicsChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,?;.:/!^*+-_";

        /// <summary>
        /// Creates a new instance. Forbidden because the Randomizer class is not intended to be instantiated.
        /// </summary>
        private Randomizer()
        {
        }

        /// <summary>
        /// The unique Random instance
        /// </summary>
        public static Random Random { get; protected set; } = new();

        /// <summary>
        /// Randomly generates (using the Random property) a new string
        /// </summary>
        /// <param name="length">The desired length for the generated string</param>
        /// <param name="chars">Range of characters to be used to generate the string (defaults to the BasicChars property)</param>
        /// <returns>The randomly generated string</returns>
        /// <see cref="Random"/>
        /// <see cref="BasicsChars"/>
        public static string NextString(int length, IEnumerable<char> chars = BasicsChars)
        {
            StringBuilder builder = new(length);
            int charsLength = chars.Count();
            for (int i = 0; i < length; i++)
            {
                builder.Append(chars.ElementAt(Random.Next(charsLength)));
            }
            return builder.ToString();
        }
    }
}