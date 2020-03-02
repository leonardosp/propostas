using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViverBem.Domain;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Documentos;
using ViverBem.Domain.Funcionarios;
using ViverBem.Domain.Propostas;
using ViverBem.Domain.Token;
using ViverBem.Infra.Data.Extensions;
using ViverBem.Infra.Data.Mappings;

namespace ViverBem.Infra.Data.Context
{
    public class ClientesContext : DbContext
    {
        public DbSet<Cliente> Clientes { get;  set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Dependente> Dependentes { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboPrinc> CombosPrinc { get; set; }
        public DbSet<DocumentoConfiguracao> DocumentoConfiguracao { get; set; }
        public DbSet<DocumentoConfiguracaoItem> DocumentoConfiguracaoItem { get; set; }
        public DbSet<LoginTokenResult> Token { get; set; }
        public DbSet<Propostas> Propostas { get; set; }
        public DbSet<DebitoEmConta> DebitoEmConta { get; set; }
        public DbSet<Profissao> Profissao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.AddConfiguration(new ClienteMapping());
            modelBuilder.AddConfiguration(new EnderecoMapping());
            modelBuilder.AddConfiguration(new FuncionarioMapping());
            modelBuilder.AddConfiguration(new DependenteMapping());
            modelBuilder.AddConfiguration(new DocumentoConfiguracaoMapping());
            modelBuilder.AddConfiguration(new DocumentoConfiguracaoItemMapping());
            modelBuilder.AddConfiguration(new ComboMapping());
            modelBuilder.AddConfiguration(new ComboPrincMapping());
            modelBuilder.AddConfiguration(new LoginTokenResultMapping());
            modelBuilder.AddConfiguration(new PropostasMapping());
            modelBuilder.AddConfiguration(new DebitoEmContaMapping());
            modelBuilder.AddConfiguration(new ProfissaoMapping());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
