using Mapster;
using TechLanches.Inativacao.DynamoDB.Models;
using TechLanchesLambda.DTOs;

namespace TechLanchesLambda.Presenter
{
    public interface IUsuarioInativoPresenter
    {
        UsuarioInativacaoDto ParaDto(UsuarioInativoDbModel usuarioInativoModel);
        UsuarioInativoDbModel ParaModel(UsuarioInativacaoDto usuarioInativoDto);
    }

    public class UsuarioInativoPresenter : IUsuarioInativoPresenter
    {
        public UsuarioInativacaoDto ParaDto(UsuarioInativoDbModel usuarioInativoModel)
            => usuarioInativoModel.Adapt<UsuarioInativacaoDto>();

        public UsuarioInativoDbModel ParaModel(UsuarioInativacaoDto usuarioInativoDto)
        {
            return new UsuarioInativoDbModel(
                usuarioInativoDto.Cpf,
                usuarioInativoDto.Nome,
                usuarioInativoDto.Email,
                usuarioInativoDto.Endereco,
                usuarioInativoDto.Telefone);
        }
    }
}
