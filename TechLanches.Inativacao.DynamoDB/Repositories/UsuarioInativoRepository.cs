using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using TechLanches.Inativacao.DynamoDB.Models;

namespace TechLanches.Inativacao.DynamoDB.Repositories
{
    public interface IUsuarioInativoRepository : IRepository<UsuarioInativoDbModel>
    {
        Task<UsuarioInativoDbModel> Cadastrar(UsuarioInativoDbModel usuarioInativo);
        Task<UsuarioInativoDbModel> BuscarUsuarioInativoPorCpf(string cpf);
    }

    public class UsuarioInativoRepository : IUsuarioInativoRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        public UsuarioInativoRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
            _dynamoDbClient = dynamoDbClient;
        }

        public UsuarioInativoRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<UsuarioInativoDbModel> Cadastrar(UsuarioInativoDbModel usuarioInativo)
        {
            await _context.SaveAsync(usuarioInativo);

            return usuarioInativo;
        }

        public async Task<UsuarioInativoDbModel> BuscarUsuarioInativoPorCpf(string cpf)
        {
            AsyncSearch<UsuarioInativoDbModel> query = _context.QueryAsync<UsuarioInativoDbModel>(cpf, new DynamoDBOperationConfig
            {
                IndexName = "cpfIndex"
            });

            UsuarioInativoDbModel usuarioInativoDynamoModel = (await query.GetNextSetAsync()).FirstOrDefault();

            if (usuarioInativoDynamoModel is null) return null;

            var usuarioInativo = new UsuarioInativoDbModel(
                usuarioInativoDynamoModel.Cpf,
                usuarioInativoDynamoModel.Nome,
                usuarioInativoDynamoModel.Email,
                usuarioInativoDynamoModel.Endereco,
                usuarioInativoDynamoModel.Telefone);

            return usuarioInativo;
        }
    }
}
