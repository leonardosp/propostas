using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViverBem.Domain.Combos;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class ComboMapping : EntityTypeConfiguration<Combo>
    {
        public override void Map(EntityTypeBuilder<Combo> builder)
        {
            builder.Property(e => e.CodCombo).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.CodComissUsr).HasColumnType("varchar(150)");
            builder.Property(e => e.PlanoSrv).HasColumnType("varchar(150)");
            builder.Property(e => e.PlaPrinServ).HasColumnType("varchar(max)");
            builder.Property(e => e.VlrCapital).HasColumnType("varchar(150)");
            builder.Property(e => e.VlrPremio).HasColumnType("varchar(150)");
            builder.Ignore(e => e.ValidationResult);
            builder.ToTable("Combos");
            builder.HasOne(e => e.ComboPrinc).WithMany(o => o.GetCombos).HasForeignKey(e => e.ComboPrincId);
        }
    }
}
