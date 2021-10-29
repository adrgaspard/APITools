using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace APITools.Core.Tools
{
    /// <summary>
    /// Represents a set of methods to retrieve some values from the configuration file.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets a boolean value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static bool GetBool(string key, bool defaultResult = false)
        {
            if (bool.TryParse(GetValue(key), out bool result))
            {
                return result;
            }
            return defaultResult;
        }

        /// <summary>
        /// Gets a double value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static double GetDouble(string key, double defaultResult = double.MinValue)
        {
            if (double.TryParse(GetValue(key), out double result))
            {
                return result;
            }
            return defaultResult;
        }

        /// <summary>
        /// Gets a float value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static float GetFloat(string key, float defaultResult = float.MinValue)
        {
            if (float.TryParse(GetValue(key), out float result))
            {
                return result;
            }
            return defaultResult;
        }

        /// <summary>
        /// Gets a int value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static int GetInt(string key, int defaultResult = int.MinValue)
        {
            if (int.TryParse(GetValue(key), out int result))
            {
                return result;
            }
            return defaultResult;
        }

        /// <summary>
        /// Gets a IEnumerable value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static IEnumerable<string> GetList(string key, bool defaultResultIsNewList = false)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                if (defaultResultIsNewList)
                {
                    return new List<string>();
                }
                return null;
            }
            return value.Split(",").Select(str => str.Trim()).ToList();
        }

        /// <summary>
        /// Gets a long value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static long GetLong(string key, long defaultResult = long.MinValue)
        {
            if (long.TryParse(GetValue(key), out long result))
            {
                return result;
            }
            return defaultResult;
        }

        /// <summary>
        /// Gets a string value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="defaultResult">The default value to return if the key was not found</param>
        /// <returns>The retrieved value or the default result if the key was not found</returns>
        public static string GetString(string key, string defaultResult = null)
        {
            string result = GetValue(key);
            if (result is not null)
            {
                return result;
            }
            return defaultResult;
        }

        /// <summary>
        /// Gets a value for a given key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <returns>The retrieved value at string format (or null if not found)</returns>
        private static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}