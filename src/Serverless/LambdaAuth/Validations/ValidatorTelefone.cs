using System.Text.RegularExpressions;

namespace TechLanchesLambda.Validations;

public static class ValidatorTelefone
{
    public static bool Validar(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            return false;

        try
        {
            string pattern = @"^\+[1-9][0-9]{0,24}$";
            return Regex.IsMatch(telefone, pattern, RegexOptions.None, TimeSpan.FromSeconds(30));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}