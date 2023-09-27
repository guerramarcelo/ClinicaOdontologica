using Clinica.API.Inputs;
using Dapper;
using System.Data.SqlClient;

namespace Clinica.API.Repositories
{
    public class ConsultaRepository
    {
        private readonly string _connectionString;
        private readonly ClinicaRepository _clinicaRepository;
        private readonly DentistaRepository _dentistaRepository;
        private readonly PacienteRepository _pacienteRepository;
        public ConsultaRepository(IConfiguration configuration, ClinicaRepository clinicaRepository, PacienteRepository pacienteRepository, DentistaRepository dentistaRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _pacienteRepository = pacienteRepository;
            _clinicaRepository = clinicaRepository;
            _dentistaRepository = dentistaRepository;
        }

        public Consulta MarcarConsulta(ConsultaInput input)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                ClinicaOdontologica clinica = _clinicaRepository.ObterClinicaPorId(input.ClinicaId);
                if (clinica == null)
                    throw new Exception("clinica não encontrada!");
                Dentista dentista = _dentistaRepository.ObterDentistaPorId(input.DentistaId);
                if (dentista == null)
                    throw new Exception("dentista não encontrado!");
                Paciente paciente = _pacienteRepository.ObterPacientePorId(input.PacienteId);
                if (paciente == null)
                    throw new Exception("paciente não encontrado!");
                Consulta consulta = new Consulta(input.Data, clinica, dentista, paciente);
                var parameters = new
                {
                    id = consulta.Id,
                    data = consulta.Data,
                    clinicaId = consulta.Clinica.Id,
                    dentistaId = consulta.Dentista.Id,
                    pacienteId = consulta.Paciente.Id,
                    status = consulta.Status
                };

                var sql = "INSERT INTO [Consulta] (Id, Data, ClinicaId, DentistaId, PacienteId, Status) VALUES" +
                "(@id, @data, @clinicaId, @dentistaId, @pacienteId, @status)";
                var result = connection.Query<Consulta>(sql, parameters);
                return consulta;
            }
        }

        public IEnumerable<Consulta> GetConsultas()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM [Consulta] co " +
                    "INNER JOIN Dentista d " +
                    "ON d.Id = co.DentistaId " +
                    "INNER JOIN Clinica c " +
                    "ON c.Id = co.ClinicaId " +
                    "INNER JOIN Paciente p " +
                    "ON p.Id = co.PacienteId";
                var consultas = connection.Query<Consulta, Dentista, ClinicaOdontologica, Paciente, Consulta>(sql,
                    (consulta, dentista, clinica, paciente) =>
                    {
                        consulta.Dentista = dentista;
                        consulta.Clinica = clinica;
                        consulta.Paciente = paciente;
                        return consulta;
                    }
                    );
                return consultas;
            }
        }

        public Consulta GetConsultaPorId(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM [Consulta] co " +
                    "INNER JOIN Dentista d " +
                    "ON d.Id = co.DentistaId " +
                    "INNER JOIN Clinica c " +
                    "ON c.Id = co.ClinicaId " +
                    "INNER JOIN Paciente p " +
                    "ON p.Id = co.PacienteId "+
                    "WHERE co.Id = @id";
                var parameters = new {id = id };
                var consulta = connection.Query<Consulta, Dentista, ClinicaOdontologica, Paciente, Consulta>(sql,
                   (consulta, dentista, clinica, paciente) =>
                   {
                       consulta.Dentista = dentista;
                       consulta.Clinica = clinica;
                       consulta.Paciente = paciente;
                       return consulta;
                   }, parameters
                   ).FirstOrDefault();
                return consulta;
            }
        }

        public Consulta RemarcarConsulta(Guid id, DateTime data)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT Id, Data, ClinicaId, DentistaId, PacienteId, Status FROM [Consulta] WHERE Id = @id";
                var parameters = new { id = id};
                Consulta consulta = connection.Query<Consulta>(sql, parameters).FirstOrDefault();
                consulta.Remarcar(data);

                sql = "UPDATE [Consulta] SET Data = @data, Status = @status WHERE Id = @id";
                var parametersUpdate = new { id = id, data = consulta.Data, status = consulta.Status};
                connection.Query(sql, parametersUpdate);
                return consulta;
            }
        }

        public Consulta CancelarConsulta(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT Id, Data, ClinicaId, DentistaId, PacienteId, Status FROM [Consulta] WHERE Id = @id";
                var parameters = new { id = id };
                Consulta consulta = connection.Query<Consulta>(sql, parameters).FirstOrDefault();
                consulta.Cancelar();

                sql = "UPDATE [Consulta] SET Status = @status WHERE Id = @id";
                var parametersUpdate = new { id = id, status = consulta.Status };
                connection.Query(sql, parametersUpdate);
                return consulta;
            }
        }
    }
}
