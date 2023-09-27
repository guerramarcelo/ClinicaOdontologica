using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Clinica.API.Repositories
{    
    public class PacienteRepository
    {
        private readonly string _connectionString;  

        public PacienteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Paciente ObterPacientePorId(Guid pacienteId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();                                
                string query = "SELECT * FROM Paciente WHERE Id = @PacienteId";
                var parameters = new { PacienteId = pacienteId };
                Paciente paciente = dbConnection.QueryFirstOrDefault<Paciente>(query, parameters);
                return paciente;
            }
        }
    }

}
