using Clinica.API.Inputs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Clinica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClinicaController : Controller
    {
        string connectionString = "Server = localhost\\MSSQLSERVER01;Database=ClinicaDB;Trusted_Connection=True";        

        [HttpPost()]
        public ActionResult CriarClinica(ClinicaInput input)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                ClinicaOdontologica clinica = new(input.Nome, input.Email, input.Cnpj,
                    new Endereco(input.Logradouro, input.Cidade, input.Estado, input.Pais));
                
                var parameters = new
                {
                    id = clinica.Id,
                    nome = clinica.Nome,
                    email = clinica.Email,
                    logradouro = clinica.Endereco.Logradouro,
                    cidade = clinica.Endereco.Cidade,
                    estado = clinica.Endereco.Estado,
                    pais = clinica.Endereco.Pais,
                    cnpj = clinica.Cnpj
                };
                var sql = "INSERT INTO [Clinica] (Id, Nome, Email, Logradouro, Cidade, Estado, Pais, Cnpj) VALUES" +
                    "(@id, @nome, @email, @logradouro, @cidade, @estado, @pais, @cnpj)";

                var result = connection.Query(sql, parameters);
                return Ok();
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetClinica(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new { Id = id };
                var sql = "select * from [Clinica] where Id = @id ";
                var result = connection.Query(sql, parameters);
                return Ok(result);
            }
        }

        [HttpGet()]
        public ActionResult GetClinicas()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT Id, Nome, Email, Cnpj, Logradouro, Cidade, Estado, Pais FROM [Clinica]";
                var clinicas = connection.Query<ClinicaOdontologica, Endereco , ClinicaOdontologica>(sql, 
                    (clinica, endereco) =>
                    {
                        clinica.Endereco = endereco;
                        return clinica;
                    }, splitOn: "Logradouro"
                    );
                return Ok(clinicas);
            }
        }

        [HttpDelete("id")]
        public ActionResult DeleteClinica(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new { Id = id };
                var sql = "DELETE FROM [Clinica] WHERE Id = @id";
                var affectedRows = connection.Execute(sql, parameters);
                return Ok(affectedRows);
            }
        }


        [HttpPut("id")]
        public ActionResult UpdateClinica(Guid id, [FromBody] ClinicaInput input)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    id = id,
                    nome = input.Nome,
                    email = input.Email,                    
                    logradouro = input.Logradouro,
                    cidade = input.Cidade,
                    estado = input.Estado,
                    pais = input.Pais,
                    cnpj = input.Cnpj
                };

                var sql = "UPDATE [Clinica] SET " +                   
                    "Nome = @nome, " +
                    "Email = @email," +                    
                    "Logradouro = @logradouro," +
                    "Cidade = @cidade," +
                    "Estado = @estado," +
                    "Pais = @pais," +
                    "Cnpj = @cnpj"+
                    " WHERE Id = @id";
                var result = connection.Query(sql, parameters);
                return Ok();
            }
        }
    }
}
