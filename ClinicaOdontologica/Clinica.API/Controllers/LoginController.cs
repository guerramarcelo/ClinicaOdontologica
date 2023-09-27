using Clinica.API.Inputs;
using Clinica.Entidades;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Clinica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        string connectionString = "Server = localhost\\MSSQLSERVER01;Database=ClinicaDB;Trusted_Connection=True";        

        [HttpGet()]
        public ActionResult FazerLogin(string email, string senha)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = "SELECT * FROM [Dentista] WHERE Email = @email";
                var parameters = new {email = email};
                var dentista = connection.Query<Dentista>(sql, parameters).FirstOrDefault();
                Pessoa pessoa = dentista;
                if(dentista == null) 
                {
                    var sqlPaciente = "SELECT * FROM [Paciente] WHERE Email = @email";
                    parameters = new { email = email };
                    var paciente = connection.Query<Paciente>(sqlPaciente, parameters).FirstOrDefault();
                    pessoa = paciente;
                    
                }
                if (pessoa == null)
                {
                    return Unauthorized();
                }
                var result = pessoa.Autenticar(email, senha);
                if(result == false)
                {
                    return Unauthorized();
                }
                return Ok("Login realizado com sucesso!");
            }
        }

        [HttpPut("{email}/alterarsenha")]
        public ActionResult AlterarSenha(string email, string senhaAntiga, string novaSenha)
        {
            using (var connection = new SqlConnection(connectionString))
            {                
                var sqlDentista = "SELECT * FROM [Dentista] WHERE Email = @email";
                var parameters = new { email = email };
                var dentista = connection.Query<Dentista>(sqlDentista, parameters).FirstOrDefault();
                Pessoa pessoa = dentista;

                if (dentista == null)
                {
                    var sqlPaciente = "SELECT * FROM [Paciente] WHERE Email = @email";
                    var paciente = connection.Query<Paciente>(sqlPaciente, parameters).FirstOrDefault();
                    pessoa = paciente;

                }
                if (pessoa == null)
                {
                    return NotFound("Usuário não encontrado");
                }
                pessoa.AlterarSenha(senhaAntiga, novaSenha);
                if (dentista != null)
                {
                    var sqlUpdateDentista = "UPDATE [Dentista] SET Senha = @novaSenha WHERE Email = @email";
                    var parametersUpdateDentista = new { email = email, novaSenha = pessoa.Senha };
                    int linhasAfetadas = connection.Execute(sqlUpdateDentista, parametersUpdateDentista);

                    if (linhasAfetadas == 0)
                    {
                        return BadRequest("Falha ao atualizar a senha do Dentista");
                    }
                }
                else 
                {
                    var sqlUpdatePaciente = "UPDATE [Paciente] SET Senha = @novaSenha WHERE Email = @email";
                    var parametersUpdatePaciente = new { email = email, novaSenha = pessoa.Senha};
                    int linhasAfetadas = connection.Execute(sqlUpdatePaciente, parametersUpdatePaciente);

                    if (linhasAfetadas == 0)
                    {
                        return BadRequest("Falha ao atualizar a senha do Paciente");
                    }
                }

                return Ok("Senha alterada com sucesso");
            }
        }
    }
}

