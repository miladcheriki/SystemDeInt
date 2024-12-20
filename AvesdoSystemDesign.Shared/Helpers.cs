using System.Text.RegularExpressions;

namespace AvesdoSystemDesign.Shared;

public static class Helpers
{
    public static bool IsValidEmail(string email)
    {
        var _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        return !string.IsNullOrEmpty(email) && _emailRegex.IsMatch(email);
    }
}