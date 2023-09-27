public class Consulta
{
    public Guid Id { get; private set; }
    public DateTime Data { get; private set; }
    public ClinicaOdontologica Clinica { get;  set; }
    public Dentista Dentista { get;   set; }
    public Paciente Paciente { get;  set; }
    public StatusConsulta Status { get;  private set; }

    public enum StatusConsulta
    {
        Marcada =1,
        Remarcada =2,
        Cancelada =3
    }

    public Consulta(DateTime data, ClinicaOdontologica clinica, Dentista dentista, Paciente paciente)
    {
        Id = Guid.NewGuid();
        Data = data;
        Clinica = clinica;
        Dentista = dentista;
        Paciente = paciente;
        Status = StatusConsulta.Marcada;
    }

    public void Remarcar(DateTime data)
    {
        if(data < DateTime.Now)
        {
            throw new Exception("Não é possivel remarcar para a data especificada!");
        }
        Status = StatusConsulta.Remarcada;
        Data = data;
    }

    public void Cancelar()
    {
        if(Status == StatusConsulta.Cancelada)
        {
            throw new Exception("A consulta já foi cancelada!");
        }
        Status = StatusConsulta.Cancelada;
    }

    public Consulta(){}
}