using Clinica.Entidades;

namespace ClinicaOdontologica.Entidades
{
    public class Paciente : Pessoa
    {
        public List<Consulta> Consultas { get; private set; }

        public Paciente(string nome, Endereco endereco, string telefone, string senha, string email)
            : base(nome, endereco, telefone, senha, email)
        {
            Consultas = new List<Consulta>();
        }
        public Paciente() { }

    }
}