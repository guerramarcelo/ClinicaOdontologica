using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Clinica.API.Repositories
{
    public class DentistaRepository
    {
        private readonly string _connectionString;

        public DentistaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Dentista ObterDentistaPorId(Guid dentistaId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                string query = "SELECT * FROM Dentista WHERE Id = @DentistaId";
                var parameters = new { DentistaId = dentistaId };
                Dentista dentista = dbConnection.QueryFirstOrDefault<Dentista>(query, parameters);
                return dentista;
            }
        }
    }

}