public class Endereco
{
    public string Logradouro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Pais { get; private set; }

    public Endereco(string logradouro, string cidade, string estado, string pais)
    {
        Logradouro = logradouro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }
    public Endereco(){}
}