using Clinica.API.Inputs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Clinica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacienteController : Controller
    {
        string connectionString = "Server = localhost\\MSSQLSERVER01;Database=ClinicaDB;Trusted_Connection=True";

        [HttpPost()]
        public ActionResult CriarPaciente(PacienteInput input)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Paciente paciente = new Paciente(input.Nome, new Endereco(input.Logradouro, input.Cidade, input.Estado, input.Pais),
                    input.Telefone, input.Senha, input.Email);
                var parameters = new
                {
                    id = paciente.Id,
                    nome = paciente.Nome,
                    email = paciente.Email,
                    telefone = paciente.Telefone,
                    senha = paciente.Senha,
                    logradouro = paciente.Endereco.Logradouro,
                    cidade = paciente.Endereco.Cidade,
                    estado = paciente.Endereco.Estado,
                    pais = paciente.Endereco.Pais
                };
                var sql = "INSERT INTO [Paciente] (Id, Nome, Email, Telefone, " +
                    "Senha, Logradouro, Cidade, Estado, Pais) VALUES" +
                    "(@id, @nome, @email, @telefone, @senha, @logradouro, @cidade, @estado, @pais)";

                var result = connection.Query(sql, parameters);
                return Ok();
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetPaciente(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new { Id = id };
                var sql = "select * from [Paciente] where Id = @id ";
                var result = connection.Query(sql, parameters);
                return Ok(result);
            }
        }

        [HttpGet()]
        public ActionResult GetPacientes()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT Id, Nome, Email, Telefone, Senha, Logradouro, Cidade, Estado, Pais FROM [Paciente]";
                var pacientes = connection.Query<Paciente, Endereco, Paciente>(sql,
                    (paciente, endereco) =>
                    {
                        paciente.Endereco = endereco;
                        return paciente;
                    }, splitOn: "Logradouro");
                return Ok(pacientes);
            }
        }

        [HttpDelete("id")]
        public ActionResult DeletePaciente(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new { Id = id };
                var sql = "DELETE FROM [Paciente] WHERE Id = @id";
                var affectedRows = connection.Execute(sql, parameters);
                return Ok(affectedRows);
            }
        }

        [HttpPut("id")]
        public ActionResult UpdatePaciente(Guid id, [FromBody] PacienteInput input)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new
                {
                    id = id,
                    nome = input.Nome,
                    email = input.Email,
                    telefone = input.Telefone,
                    logradouro = input.Logradouro,
                    cidade = input.Cidade,
                    estado = input.Estado,
                    pais = input.Pais
                };
                var sql = "UPDATE [Paciente] SET " +
                    "Nome = @nome, " +
                    "Email = @email," +
                    "Telefone = @telefone," +
                    "Logradouro = @logradouro," +
                    "Cidade = @cidade," +
                    "Estado = @estado," +
                    "Pais = @pais" +
                    " WHERE Id = @id";
                var result = connection.Query(sql, parameters);
                return Ok();
            }
        }
    }
}
