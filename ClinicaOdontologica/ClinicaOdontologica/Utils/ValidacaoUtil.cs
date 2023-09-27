using System.Text.RegularExpressions;

namespace Clinica.Utils
{
    public static class ValidacaoUtil
    {
        public static bool ValidaEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        public static bool ValidaCnpj(string cnpj)
        {
            string pattern = @"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$";
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(cnpj))
                return false;
            return true;
        }

        public static bool ValidaTelefone(string telefone)
        {
            var count = telefone.Count();
            if (count >= 8 && count <= 11)
            {
                return true;
            }
            return false;
        }
    }
}
