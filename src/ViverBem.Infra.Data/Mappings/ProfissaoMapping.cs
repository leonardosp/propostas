using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class ProfissaoMapping : EntityTypeConfiguration<Profissao>
    {
        public override void Map(EntityTypeBuilder<Profissao> builder)
        {
            builder.Property(e => e.Id).HasColumnType("varchar(150");
            builder.Property(e => e.profcod).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.profdesc).HasColumnType("varchar(150)");
            builder.ToTable("Profissao");
        }
    }
}
