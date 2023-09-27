using System.Text;
using System.Security.Cryptography;

namespace Clinica.Utils
{
    public static class CriptografiaUtil
    {
        public static string CriptografarSenha(string senha)
        {
            var hash = new SHA1CryptoServiceProvider();
            var senhaBytes = Encoding.Default.GetBytes(senha);
            var senhaHash = hash.ComputeHash(senhaBytes);
            var senhaHashBase64 = Convert.ToBase64String(senhaHash);

            return senhaHashBase64;
        }
    }
}
