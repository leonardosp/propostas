using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Clientes;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class DependenteMapping : EntityTypeConfiguration<Dependente>
    {
        public override void Map(EntityTypeBuilder<Dependente> builder)
        {
            builder.Property(e => e.Nome).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.Parentesco).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.Participacao).HasColumnType("decimal").IsRequired();
            builder.Property(e => e.DataNascimento).HasColumnType("date").IsRequired();
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);
            builder.ToTable("Dependentes");
            builder.HasOne(e => e.Cliente).WithMany(o => o.Dependentes).HasForeignKey(e => e.ClienteId);
        }
    }
}
