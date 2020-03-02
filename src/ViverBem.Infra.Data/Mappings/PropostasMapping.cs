using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Propostas;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class PropostasMapping : EntityTypeConfiguration<Propostas>
    {
        public override void Map(EntityTypeBuilder<Propostas> builder)
        {
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);
            builder.ToTable("Propostas");
            builder.HasOne(e => e.Funcionario).WithMany(o => o.Propostas).HasForeignKey(e => e.FuncionarioId);
            builder.Ignore(e => e.Cliente);
            builder.Ignore(e => e.Combo);
        }
    }
}
