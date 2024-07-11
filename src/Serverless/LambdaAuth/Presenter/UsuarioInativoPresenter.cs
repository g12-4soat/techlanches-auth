using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            => usuarioInativoDto.Adapt<UsuarioInativoDbModel>();
    }
}
