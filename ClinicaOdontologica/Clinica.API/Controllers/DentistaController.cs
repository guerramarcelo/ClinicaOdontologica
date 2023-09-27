using Clinica.API.Inputs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Clinica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DentistaController : Controller
    {
        string connectionString = "Server = localhost\\MSSQLSERVER01;Database=ClinicaDB;Trusted_Connection=True";

        [HttpPost()]
        public ActionResult CriarDentista(DentistaInput input)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Dentista dentista = new Dentista(input.Nome,
                    new Endereco(input.Logradouro, input.Cidade, input.Estado, input.Pais),
                    input.Telefone, input.Senha, input.Email, input.Registro);


                var parameters = new
                {
                    id = dentista.Id,
                    nome = dentista.Nome,
                    email = dentista.Email,
                    telefone = dentista.Telefone,
                    senha = dentista.Senha,
                    logradouro = dentista.Endereco.Logradouro,
                    cidade = dentista.Endereco.Cidade,
                    estado = dentista.Endereco.Estado,
                    pais = dentista.Endereco.Pais,
                    registro = dentista.Registro
                };
                var sql = "INSERT INTO [Dentista] (Id, Nome, Email, Telefone, Senha, Logradouro, Cidade, Estado, Pais, Registro) VALUES" +
                    "(@id, @nome, @email, @telefone, @senha, @logradouro, @cidade, @estado, @pais, @registro)";

                var result = connection.Query(sql, parameters);
                return Ok();
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetDentista(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new { Id = id };
                var sql = "select * from [Dentista] where Id = @id ";
                var result = connection.Query(sql, parameters);
                return Ok(result);
            }
        }

        [HttpGet()]
        public ActionResult GetDentistas()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT Id, Registro, Nome, Email, Telefone, Senha, Logradouro, Cidade, Estado, Pais FROM [Dentista]";
                var dentistas = connection.Query<Dentista, Endereco, Dentista>(sql,
                    (dentista, endereco) =>
                    {
                        dentista.Endereco = endereco;
                        return dentista;
                    }, splitOn: "Logradouro");
                return Ok(dentistas);
            }
        }

        [HttpDelete("id")]
        public ActionResult DeleteDentista(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new { Id = id };
                var sql = "DELETE FROM [Dentista] WHERE Id = @id";
                var affectedRows = connection.Execute(sql, parameters);
                return Ok(affectedRows);
            }
        }

        [HttpPut("id")]
        public ActionResult UpdateDentista(Guid id, [FromBody] DentistaInput input)
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
                    pais = input.Pais,
                    registro = input.Registro
                };
                var sql = "UPDATE [Dentista] SET " +
                    "Nome = @nome, " +
                    "Email = @email," +
                    "Telefone = @telefone," +
                    "Logradouro = @logradouro," +
                    "Cidade = @cidade," +
                    "Estado = @estado," +
                    "Pais = @pais," +
                    "Registro = @registro" +
                    " WHERE Id = @id";
                var result = connection.Query(sql, parameters);
                return Ok();
            }
        }
    }
}