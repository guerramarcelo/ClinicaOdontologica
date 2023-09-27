using Clinica.Utils;

namespace Clinica.Entidades
{
    public abstract class Pessoa
    {
        public string Nome { get; private set; }
        public Endereco Endereco { get; set; }
        public string Telefone { get; private set; }
        public Guid Id { get; set; }
        public string Senha { get; private set; }
        public string Email { get; private set; }

        protected Pessoa(string nome, Endereco endereco, string telefone, string senha, string email)
        {
            if (!ValidacaoUtil.ValidaEmail(email))
                throw new Exception("Email inválido!");
            if (!ValidacaoUtil.ValidaTelefone(telefone))
                throw new Exception("Telefone inválido!");

            Nome = nome;
            Endereco = endereco;
            Telefone = telefone;
            Id = Guid.NewGuid();
            Senha = CriptografiaUtil.CriptografarSenha(senha);
            Email = email;
        }
        public Pessoa(){}

        public bool Autenticar(string email, string senha)
        {
            if ((Email == email) && (Senha == CriptografiaUtil.CriptografarSenha(senha)))
            {
                return true;
            }
            return false;
        }

        public void AlterarTelefone(string novoTelefone)
        {
            Telefone = novoTelefone;
        }

        public void EditarEndereco(Endereco novoEndereco)
        {
            Endereco = novoEndereco;
        }

        public void AlterarSenha(string senhaAntiga, string novaSenha)
        {
            if (Senha == Utils.CriptografiaUtil.CriptografarSenha(senhaAntiga))
            {
                Senha = Utils.CriptografiaUtil.CriptografarSenha(novaSenha);
            }
            else
            {
                throw new Exception("Senha incorreta!");
            }
        }

        public void AlterarEmail(string novoEmail)
        {
            Email = novoEmail;
        }
    }
}