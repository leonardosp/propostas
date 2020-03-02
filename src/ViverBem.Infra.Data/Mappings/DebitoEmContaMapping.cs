using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class DebitoEmContaMapping : EntityTypeConfiguration<DebitoEmConta>
    {
        public override void Map(EntityTypeBuilder<DebitoEmConta> builder)
        {
            builder.Property(e => e.Id).HasColumnType("varchar(150");
            builder.Property(e => e.bancod).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.bannome).HasColumnType("varchar(150)");
            builder.ToTable("DebitoEmConta");
        }
    }
}
