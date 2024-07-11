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
        public string Id { get; set; } = default!;
        public string Cpf { get; set; } = default!;
        public string Nome { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Endereco { get; set; } = default!;
        public string Telefone { get; set; } = default!;
    }
}
