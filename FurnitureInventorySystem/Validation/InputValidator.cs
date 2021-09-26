using System.Text.RegularExpressions;

namespace FurnitureInventorySystem.Validation
{
    public class InputValidator
    {
        public InputValidator()
        {
        }

        public static bool ValidString(string text)
        {
            // String containing only letters, whitespace and non null.
            var regex = new Regex("^[a-zA-Z_ ]+$");

            return regex.Match(text).Success;
        }

        public static bool ValidInteger(string text)
        {
            // Any real positive integer
            var regex = new Regex("^(0|[1-9]\\d*)$");

            return regex.Match(text).Success;
        }

        public static bool ValidDecimal(string text)
        {
            // Two decimal places required for money.
            var regex = new Regex("^[^0|\\D]\\d{0,9}(\\.\\d{2,2})$");

            return regex.Match(text).Success;
        }
    }
}