using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Funcionarios;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class FuncionarioMapping : EntityTypeConfiguration<Funcionario>
    {
        public override void Map(EntityTypeBuilder<Funcionario> builder)
        {
            builder.Property(e => e.Nome).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.Email).HasColumnType("varchar(100)").IsRequired();
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);
            builder.ToTable("Funcionarios");
    
        }
    }
}
