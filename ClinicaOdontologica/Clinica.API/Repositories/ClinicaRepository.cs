using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Clinica.API.Repositories
{
    public class ClinicaRepository
    {
        private readonly string _connectionString;

        public ClinicaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public ClinicaOdontologica ObterClinicaPorId(Guid clinicaId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();                                
                string query = "SELECT * FROM Clinica WHERE Id = @ClinicaId";
                var parameters = new { ClinicaId = clinicaId };
                ClinicaOdontologica clinica = dbConnection.QueryFirstOrDefault<ClinicaOdontologica>(query, parameters);

                return clinica;
            }
        }
    }

}
