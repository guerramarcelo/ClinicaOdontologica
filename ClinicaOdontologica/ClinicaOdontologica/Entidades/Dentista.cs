using Clinica.Entidades;

namespace ClinicaOdontologica.Entidades
{
    public class Dentista : Pessoa
    {
        public string Registro { get; private set; }

        public Dentista(string nome, Endereco endereco, string telefone, string senha, string email, string registro)
            : base(nome, endereco, telefone, senha, email)
        {
            Registro = registro;
        }
        public Dentista() { }
    }
}