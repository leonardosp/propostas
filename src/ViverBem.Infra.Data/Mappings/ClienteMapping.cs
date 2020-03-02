using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Funcionarios;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class ClienteMapping : EntityTypeConfiguration<Cliente>
    {
        public override void Map(EntityTypeBuilder<Cliente> builder)
        {
            builder.Property(e => e.Nome).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.Celular).HasColumnType("varchar(150)");
            builder.Property(e => e.Fone).HasColumnType("varchar(150)");
            builder.Property(e => e.RG).HasColumnType("varchar(max)");
            builder.Property(e => e.OrgaoExpedidor).HasColumnType("varchar(150)");
            builder.Property(e => e.CPF).HasColumnType("varchar(150)");
            builder.Property(e => e.Email).HasColumnType("varchar(150)");
            builder.Ignore(e => e.ValidationResult);
            builder.ToTable("Clientes");
            builder.HasOne(e => e.Funcionario).WithMany(o => o.Clientes).HasForeignKey(e => e.FuncionarioId);
            builder.Ignore(e => e.Dependente);
        }
    }
}
