using AutoMapper;
using ViverBem.Application.ViewModels;
using ViverBem.Domain;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Documentos;
using ViverBem.Domain.Funcionarios;
using ViverBem.Domain.Propostas;
using ViverBem.Domain.Token;

namespace ViverBem.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Cliente, ClienteViewModel>();
            CreateMap<Endereco, EnderecoViewModel>();
            CreateMap<Funcionario, FuncionarioViewModel>();
            CreateMap<DocumentoConfiguracao, DocumentoConfiguracaoViewModel>();
            CreateMap<DocumentoConfiguracaoItem, DocumentoConfiguracaoItemViewModel>();
            CreateMap<LoginTokenResult, TokenResultViewModel>();
            CreateMap<Propostas, PropostasViewModel>();
            CreateMap<Dependente, DependenteViewModel>();
            CreateMap<ComboPrinc, ComboPrincViewModel>();
            CreateMap<Combo, ComboViewModel>();
            CreateMap<DebitoEmConta, DebitoEmContaDisponivel>();
            CreateMap<Profissao, ProfissaoViewModel>();
        }
    }
}
