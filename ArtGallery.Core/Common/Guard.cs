using static ArtGallery.Core.Constants.ExceptionMessageConstants;
using static System.String;

namespace ArtGallery.Core.Common
{
    /// <summary>
    /// Holds basic validation methods.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Used to check if an object is null.
        /// </summary>
        /// <param name="value">The object to be checked.</param>
        /// <param name="name">The name of the object to be displayed in the exception message.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AgainstNull(object? value, string? name = null)
        {
            name ??= nameof(value);
            if (value == null)
                throw new ArgumentNullException(Format(GuardNullException, name));
        }
        /// <summary>
        /// Used to check if a string is null, white space or empty.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <param name="name">The name of the variable to be displayed in the exception message.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AgainstNullOrWhiteSpaceString(string? value, string? name = null)
        {
            name ??= nameof(value);
            if (IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(Format(GuardStringException, name));
        }
        /// <summary>
        /// Used to check if a number is between two other numbers.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void AgainstOutOfRange(int number, int start, int end, string name = "")
        {
            if (number < start || number > end)
                throw new ArgumentException(Format(GuardOutOfRangeException, name));
        }
    }
}
