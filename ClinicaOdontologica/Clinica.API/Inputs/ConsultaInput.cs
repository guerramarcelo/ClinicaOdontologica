namespace Clinica.API.Inputs
{
    public class ConsultaInput
    {
        public DateTime Data { get; set; }
        public Guid ClinicaId { get; set; }
        public Guid DentistaId { get; set; }
        public Guid PacienteId { get; set; }
    }
}
