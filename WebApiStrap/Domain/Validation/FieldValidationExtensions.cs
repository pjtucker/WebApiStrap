namespace WebApiStrap.Domain.Validation
{
    using System.Text.RegularExpressions;

    public static class FieldValidationExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
        }
    }
}