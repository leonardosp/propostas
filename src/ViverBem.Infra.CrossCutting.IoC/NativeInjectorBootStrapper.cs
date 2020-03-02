using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Application.Interfaces;
using ViverBem.Application.Services;
using ViverBem.Domain.Clientes.Commands;
using ViverBem.Domain.Clientes.Events;
using ViverBem.Domain.Clientes.Repository;
using ViverBem.Domain.Combos.Commands;
using ViverBem.Domain.Combos.Events;
using ViverBem.Domain.Combos.Repository;
using ViverBem.Domain.Documentos.Commands;
using ViverBem.Domain.Documentos.Events;
using ViverBem.Domain.Documentos.Repository;
using ViverBem.Domain.Funcionarios.Commands;
using ViverBem.Domain.Funcionarios.Events;
using ViverBem.Domain.Funcionarios.Repository;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Propostas.Commands;
using ViverBem.Domain.Propostas.Events;
using ViverBem.Domain.Propostas.Repository;
using ViverBem.Domain.Token.Commands;
using ViverBem.Domain.Token.Events;
using ViverBem.Domain.Token.Repository;
using ViverBem.Infra.CrossCutting.Bus;
using ViverBem.Infra.CrossCutting.Identity.Models;
using ViverBem.Infra.CrossCutting.Identity.Services;
using ViverBem.Infra.Data.Context;
using ViverBem.Infra.Data.Repository;
using ViverBem.Infra.Data.UoW;

namespace ViverBem.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {

            //ASPNET
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Application
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
            services.AddScoped<IClienteAppService, ClienteAppService>();
            services.AddScoped<IFuncionarioAppService, FuncionarioAppService>();
            services.AddScoped<IDocumentoAppService, DocumentoConfiguracaoAppService>();
            services.AddScoped<IComboAppService, ComboAppService>();
            services.AddScoped<IDocumentoAppService, DocumentoConfiguracaoAppService>();
            services.AddScoped<IPropostaAppService, PropostaAppService>();
            services.AddScoped<ITokenAppService, TokenAppService>();
            //Domain - Commands
            services.AddScoped<IHandler<RegistrarClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<AtualizarClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<ExcluirClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<RegistrarPropostasCommand>, PropostasCommandHandler>();
            services.AddScoped<IHandler<AtualizarPropostasCommand>, PropostasCommandHandler>();
            services.AddScoped<IHandler<ExcluirPropostasCommand>, PropostasCommandHandler>();
            services.AddScoped<IHandler<RegistrarFuncionarioCommand>, FuncionarioCommandHandler>();
            services.AddScoped<IHandler<RegistrarDocumentoConfiguracaoCommand>, DocumentoConfiguracaoCommandHandler>();
            services.AddScoped<IHandler<IncluirDocumentoConfiguracaoItemCommand>, DocumentoConfiguracaoCommandHandler>();
            services.AddScoped<IHandler<ExcluirDocumentoConfiguracaoCommand>, DocumentoConfiguracaoCommandHandler>();
            services.AddScoped<IHandler<ExcluirDocumentoConfiguracaoItemCommand>, DocumentoConfiguracaoCommandHandler>();
            services.AddScoped<IHandler<AtualizarDocumentoConfiguracaoCommand>, DocumentoConfiguracaoCommandHandler>();
            services.AddScoped<IHandler<AtualizarDocumentoConfiguracaoItemCommand>, DocumentoConfiguracaoCommandHandler>();
            services.AddScoped<IHandler<AtualizarEnderecoClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<IncluirEnderecoClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<RegistrarTokenCommand>, TokenCommandHandler>();
            services.AddScoped<IHandler<AtualizarTokenCommand>, TokenCommandHandler>();
            services.AddScoped<IHandler<ExcluirTokenCommand>, TokenCommandHandler>();
            services.AddScoped<IHandler<IncluirDependenteClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<AtualizarDependenteClienteCommand>, ClienteCommandHandler>();
            services.AddScoped<IHandler<RegistrarComboPrincCommand>, ComboCommandHandler>();
            services.AddScoped<IHandler<AtualizarComboPrinciCommand>, ComboCommandHandler>();
            services.AddScoped<IHandler<ExcluirComboPrincCommand>, ComboCommandHandler>();
            services.AddScoped<IHandler<IncuirComboCommand>, ComboCommandHandler>();
            services.AddScoped<IHandler<AtualizarComboCommand>, ComboCommandHandler>();
            services.AddScoped<IHandler<ExcluirComboCommand>, ComboCommandHandler>();
            //Domain - Eventos
            services.AddScoped<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<ClienteAtualizadoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<ClienteExcluidoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<PropostasRegistradoEvent>, PropostasEventHandler>();
            services.AddScoped<IHandler<PropostasAtualizadoEventHandler>, PropostasEventHandler>();
            services.AddScoped<IHandler<PropostasExcluidaEvent>, PropostasEventHandler>();
            services.AddScoped<IHandler<TokenRegistradoEvent>, TokenEventHandler>();
            services.AddScoped<IHandler<TokenAtualizadoEvent>, TokenEventHandler>();
            services.AddScoped<IHandler<TokenExcluidoEvent>, TokenEventHandler>();
            services.AddScoped<IHandler<FuncionarioRegistradoEvent>, FuncionarioEventHandler>();
            services.AddScoped<IHandler<DocumentoConfiguracaoRegistradoEvent>, DocumentoConfiguracaoEventHandler>();
            services.AddScoped<IHandler<DocumentoConfiguracaoExcluidoEvent>, DocumentoConfiguracaoEventHandler>();
            services.AddScoped<IHandler<DocumentoConfiguracaoAtualizadoEvent>, DocumentoConfiguracaoEventHandler>();
            services.AddScoped<IHandler<DocumentoConfiguracaoItemAdicionadoEvent>, DocumentoConfiguracaoEventHandler>();
            services.AddScoped<IHandler<DocumentoConfiguracaoItemAtualizadoEvent>, DocumentoConfiguracaoEventHandler>();
            services.AddScoped<IHandler<DocumentoConfiguracaoItemExcluidoEvent>, DocumentoConfiguracaoEventHandler>();
            services.AddScoped<IHandler<EnderecoClienteAtualizadoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<EnderecoClienteAdicionadoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<DependenteClienteAdicionadoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<DependenteClienteAtualizadoEvent>, ClienteEventHandler>();
            services.AddScoped<IHandler<ComboPrincRegistradoEvent>, ComboPrincEventHandler>();
            services.AddScoped<IHandler<ComboPrincAtualizadoEvent>, ComboPrincEventHandler>();
            services.AddScoped<IHandler<ComboPrincExcluidoEvent>, ComboPrincEventHandler>();
            services.AddScoped<IHandler<ComboAdicionadoEvent>, ComboPrincEventHandler>();
            services.AddScoped<IHandler<ComboAtualizadoEvent>, ComboPrincEventHandler>();
            services.AddScoped<IHandler<ComboExcluidoEvent>, ComboPrincEventHandler>();
            //Infra - Data
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IDocumentoConfiguracaoRepository, DocumentoConfiguracaoRepository>();
            services.AddScoped<IComboRepository, ComboRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IPropostasRepository, PropostaRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ClientesContext>();

            //Infra - Bus
            services.AddScoped<IBus, InMemoryBus>();

            //Infra - Identity
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUser, AspNetUser>();

        }
    }
}
