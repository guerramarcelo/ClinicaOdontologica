using Clinica.Utils;

public class ClinicaOdontologica
{
    public Guid Id { get; set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cnpj { get; private set; }
    public Endereco Endereco { get; set; }
    public List<Consulta> Consultas { get; private set; }

    public ClinicaOdontologica(string nome, string email, string cnpj, Endereco endereco)
    {
        if (!ValidacaoUtil.ValidaCnpj(cnpj))
            throw new Exception("Cnpj inválido!");

        if (!ValidacaoUtil.ValidaEmail(email))
            throw new Exception("Email inválido!");

        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Cnpj = cnpj;
        Endereco = endereco;
        Consultas = new List<Consulta>();
    }

    public ClinicaOdontologica() { }

    public void NovaConsulta(DateTime data, Paciente paciente, Dentista dentista)
    {
        Consultas.Add(new Consulta(data, this, dentista, paciente));
    }

    public void RemarcarConsulta(Guid idConsulta, DateTime novaData)
    {
        Consulta consulta = Consultas.Find(c => c.Id == idConsulta);
        if (consulta != null)
        {
            //consulta.RemarcarConsulta(novaData);
        }
    }

    public void CancelarConsulta(Guid idConsulta)
    {
        Consulta consulta = Consultas.Find(c => c.Id == idConsulta);
        if (consulta != null)
        {
            //consulta.CancelarConsulta();
        }
    }
}