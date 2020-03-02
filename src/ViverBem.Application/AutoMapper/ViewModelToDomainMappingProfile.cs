using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Clientes.Commands;
using ViverBem.Domain.Combos.Commands;
using ViverBem.Domain.Documentos.Commands;
using ViverBem.Domain.Funcionarios.Commands;
using ViverBem.Domain.Propostas.Commands;
using ViverBem.Domain.Token.Commands;

namespace ViverBem.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ClienteViewModel, RegistrarClienteCommand>()
                .ConstructUsing(c => new RegistrarClienteCommand(c.Nome,c.CPF,c.RG,c.DataExpedicao,c.DataNasc,DateTime.Now,c.OrgaoExpedidor,c.Fone,c.Celular,c.Email,c.Sexo,c.EstadoCivil,c.Ocupacao,c.CodigoProf,c.PPE,c.FuncionarioId,
                new IncluirEnderecoClienteCommand(c.Endereco.Id, c.Endereco.Logradouro, c.Endereco.Numero, c.Endereco.Complemento, c.Endereco.Bairro, c.Endereco.CEP, c.Endereco.Cidade, c.Endereco.Estado, c.Endereco.ClienteId)));

            CreateMap<EnderecoViewModel, IncluirEnderecoClienteCommand>()
                .ConstructUsing(c => new IncluirEnderecoClienteCommand(Guid.NewGuid(), c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.CEP, c.Cidade, c.Estado, c.ClienteId));

            CreateMap<EnderecoViewModel, AtualizarEnderecoClienteCommand>()
                .ConstructUsing(c => new AtualizarEnderecoClienteCommand(Guid.NewGuid(), c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.CEP, c.Cidade, c.Estado, c.ClienteId));

            CreateMap<DependenteViewModel, IncluirDependenteClienteCommand>()
                .ConstructUsing(c => new IncluirDependenteClienteCommand(Guid.NewGuid(), c.Nome, c.Parentesco, c.Participacao, c.ClienteId));

            CreateMap<DependenteViewModel, AtualizarDependenteClienteCommand>()
                .ConstructUsing(c => new AtualizarDependenteClienteCommand(Guid.NewGuid(), c.Nome, c.Parentesco, c.Participacao, c.ClienteId));

            CreateMap<PropostasViewModel, RegistrarPropostasCommand>()
                .ConstructUsing(c => new RegistrarPropostasCommand(c.FuncionarioId, c.Dt_venda,c.Corretor, c.Id_ext_props, c.Tp_pgto, c.CPF, c.Nome, c.Dt_nasc, c.Sexo, c.Est_civil, c.Ender, c.Endnum, c.EndComplemento, c.Bairro, c.Cidade, c.UF, c.CEP, c.DDDTel, c.Tel, c.DDDCel, c.Cel, c.Identidade, c.Org_emissor, c.Email, c.Cod_prof, c.Ocupacao, c.Dt_venda,DateTime.Now, c.CodCombo, c.VlrPremio, c.VlrCapital, c.Tp_pgto, c.Banco, c.Agencia, c.Agencia_dv, c.CC, c.Cc_dv, c.Dia_debito, c.Tipo_envio, c.Endercobr, c.Endnumcobr, c.Endcomplcobr, c.Bairrocobr, c.PPE, c.Cidadecobr, c.Ufcobr, c.Cepcobr,c.IdDoClienteX));

            CreateMap<PropostasViewModel, AtualizarPropostasCommand>().ConstructUsing(c => new AtualizarPropostasCommand(c.FuncionarioId,c.Corretor, c.Dt_venda, c.Id_ext_props, c.Tp_pgto, c.CPF, c.Nome, c.Dt_nasc, c.Sexo, c.Est_civil, c.Ender, c.Endnum, c.EndComplemento, c.Bairro, c.Cidade, c.UF, c.CEP, c.DDDTel, c.Tel, c.DDDCel, c.Cel, c.Identidade, c.Org_emissor, c.Email, c.Cod_prof, c.Ocupacao, c.Dt_venda,DateTime.Now, c.CodCombo, c.VlrPremio, c.VlrCapital, c.Tp_pgto, c.Banco, c.Agencia, c.Agencia_dv, c.CC, c.Cc_dv, c.Dia_debito, c.Tipo_envio, c.Endercobr, c.Endnumcobr, c.Endcomplcobr, c.Bairrocobr, c.Cidadecobr, c.Ufcobr, c.Cepcobr, c.Pronum,c.AssocMatric, c.StatusConsulta,c.StatusFinanceiro,c.ProdTPag,c.PPE,c.Aprovado,c.IdDoClienteX));

            CreateMap<PropostasViewModel, ExcluirPropostasCommand>().ConstructUsing(c => new ExcluirPropostasCommand(c.Id));

            CreateMap<ComboPrincViewModel, RegistrarComboPrincCommand>().ConstructUsing(c => new RegistrarComboPrincCommand(c.Corretor,c.CodCombo));

            CreateMap<ComboPrincViewModel, AtualizarComboPrinciCommand>().ConstructUsing(c => new AtualizarComboPrinciCommand(c.CodCombo, c.Corretor));

            CreateMap<ComboPrincViewModel, ExcluirComboPrincCommand>().ConstructUsing(c => new ExcluirComboPrincCommand(c.Id));

            CreateMap<ComboViewModel, IncuirComboCommand>().ConstructUsing(c => new IncuirComboCommand(c.CodCombo, c.PlanoSrv, Guid.NewGuid(), c.ComboPrincId.Value, c.PlaPrinServ, c.VlrPremio, c.VlrCapital, c.CodComissUsr));

            CreateMap<ComboViewModel, AtualizarComboCommand>().ConstructUsing(c => new AtualizarComboCommand(c.CodCombo, c.PlanoSrv, Guid.NewGuid(), c.ComboPrincId.Value, c.PlaPrinServ, c.VlrPremio, c.VlrCapital, c.CodComissUsr));

            CreateMap<ComboViewModel, ExcluirComboCommand>().ConstructUsing(c => new ExcluirComboCommand(c.Id));

            CreateMap<TokenResultViewModel, RegistrarTokenCommand>().ConstructUsing(c => new RegistrarTokenCommand(c.AccessToken, c.Error, c.ErrorDescription,c.RETORNOTOTAL, c.LimiteDoToken));

            CreateMap<TokenResultViewModel, AtualizarTokenCommand>().ConstructUsing(c => new AtualizarTokenCommand(Guid.NewGuid(), c.AccessToken, c.Error, c.ErrorDescription,c.RETORNOTOTAL, c.LimiteDoToken));

            CreateMap<TokenResultViewModel, ExcluirTokenCommand>().ConstructUsing(c => new ExcluirTokenCommand(c.Id));

            CreateMap<DocumentoConfiguracaoViewModel, RegistrarDocumentoConfiguracaoCommand>()
                .ConstructUsing(c => new RegistrarDocumentoConfiguracaoCommand(c.NrSeqDocumentoConfiguracao, c.CdDocumentoConfiguracao, c.NrOrdem, c.NrColuna, c.NoBackGroundColor, c.NoHorizontalAligment, c.NrTamanhoBorda, c.NrMarginLeft, c.NoPropriedadeLista, c.NrParagraphspacing, c.NrWidth));

            CreateMap<DocumentoConfiguracaoViewModel, AtualizarDocumentoConfiguracaoCommand>()
                .ConstructUsing(c => new AtualizarDocumentoConfiguracaoCommand(Guid.NewGuid(), c.CdDocumentoConfiguracao, c.NrOrdem, c.NrColuna, c.NoBackGroundColor, c.NoHorizontalAligment, c.NrTamanhoBorda, c.NrMarginLeft, c.NoPropriedadeLista, c.NrParagraphspacing, c.NrWidth));

            CreateMap<DocumentoConfiguracaoViewModel, ExcluirDocumentoConfiguracaoCommand>()
                .ConstructUsing(c => new ExcluirDocumentoConfiguracaoCommand(c.NrSeqDocumentoConfiguracao));

            CreateMap<DocumentoConfiguracaoItemViewModel, IncluirDocumentoConfiguracaoItemCommand>()
                .ConstructUsing(c => new IncluirDocumentoConfiguracaoItemCommand(c.NoFonte, c.NoFonteColor, c.NoFrase, c.FlgPropriedade, c.NrColuna, Guid.NewGuid(), c.NrFonteTamanho, c.NoTipoFonte, c.NrSeqDocumentoConfiguracao, c.FlgIdentificador, c.FlgImage, c.NrMaxImageWidth, c.NrMaxImageHeight));

            CreateMap<DocumentoConfiguracaoItemViewModel, AtualizarDocumentoConfiguracaoItemCommand>()
                .ConstructUsing(c => new AtualizarDocumentoConfiguracaoItemCommand(c.NoFonte, c.NoFonteColor, c.NoFrase, c.FlgPropriedade, c.NrColuna, Guid.NewGuid(), c.NrFonteTamanho, c.NoTipoFonte, c.NrSeqDocumentoConfiguracao, c.FlgIdentificador, c.FlgImage, c.NrMaxImageWidth, c.NrMaxImageHeight));

            CreateMap<DocumentoConfiguracaoItemViewModel, ExcluirDocumentoConfiguracaoItemCommand>()
                .ConstructUsing(c => new ExcluirDocumentoConfiguracaoItemCommand(c.NrseqDocumentoConfiguracaoItem));

            CreateMap<ClienteViewModel, AtualizarClienteCommand>()
                .ConstructUsing(c => new AtualizarClienteCommand(c.Id, c.Nome, c.CPF, c.RG, c.DataExpedicao, c.DataNasc,DateTime.Now, c.OrgaoExpedidor, c.Fone, c.Celular, c.Email,c.Sexo,c.EstadoCivil,c.Ocupacao,c.CodigoProf,c.PPE, c.FuncionarioId));

            CreateMap<ClienteViewModel, ExcluirClienteCommand>()
                .ConstructUsing(c => new ExcluirClienteCommand(c.Id));

            CreateMap<FuncionarioViewModel, RegistrarFuncionarioCommand>()
                .ConstructUsing(c => new RegistrarFuncionarioCommand(c.Id, c.Nome, c.Email));
        }
    }
}
