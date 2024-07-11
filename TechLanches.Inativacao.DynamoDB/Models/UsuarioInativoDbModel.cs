using Amazon.DynamoDBv2.DataModel;

namespace TechLanches.Inativacao.DynamoDB.Models
{
    [DynamoDBTable("usuario_inativo")]
    public class UsuarioInativoDbModel
    {
        public UsuarioInativoDbModel(string cpf, string nome, string email, string endereco, string telefone)
        {
            Id = Guid.NewGuid().ToString();
            Cpf = cpf;
            Nome = nome;
            Email = email;
            Endereco = endereco;
            Telefone = telefone;
        }
        public UsuarioInativoDbModel()
        {
        }

        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
    }
}
