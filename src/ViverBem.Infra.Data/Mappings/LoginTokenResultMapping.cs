using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Documentos;
using ViverBem.Domain.Token;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class LoginTokenResultMapping : EntityTypeConfiguration<LoginTokenResult>
    {
        public override void Map(EntityTypeBuilder<LoginTokenResult> builder)
        {
            builder.Property(e => e.AccessToken).HasColumnType("varchar(5000)").IsRequired();
            builder.Property(e => e.Error).HasColumnType("varchar(500)");
            builder.Property(e => e.ErrorDescription).HasColumnType("varchar(500)");
            builder.Property(e => e.LimiteDoToken).HasColumnType("datetime");
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);
            builder.ToTable("Tokens");
        }
    }
}
