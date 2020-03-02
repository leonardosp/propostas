using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Combos;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class ComboPrincMapping : EntityTypeConfiguration<ComboPrinc>
    {
        public override void Map(EntityTypeBuilder<ComboPrinc> builder)
        {
            builder.Property(e => e.CodCombo).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.Corretor).HasColumnType("varchar(200)").IsRequired();
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);
            builder.ToTable("CombosPrinc");
            builder.Ignore(e => e.Combo);
        }
    }
}
