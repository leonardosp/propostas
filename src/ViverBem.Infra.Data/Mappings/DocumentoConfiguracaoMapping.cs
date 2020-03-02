using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Documentos;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class DocumentoConfiguracaoMapping : EntityTypeConfiguration<DocumentoConfiguracao>
    {
        public override void Map(EntityTypeBuilder<DocumentoConfiguracao> builder)
        {
            builder.Property(e => e.CdDocumentoConfiguracao).HasColumnType("varchar(150)").IsRequired();
            builder.Property(e => e.NoBackGroundColor).HasColumnType("varchar(150)");
            builder.Property(e => e.NoHorizontalAligment).HasColumnType("varchar(150)");
            builder.Property(e => e.NoPropriedadeLista).HasColumnType("varchar(max)");
            builder.Property(e => e.NrColuna).HasColumnType("varchar(150)");
            builder.Property(e => e.NrMarginLeft).HasColumnType("varchar(150)");
            builder.Property(e => e.NrParagraphspacing).HasColumnType("varchar(150)");
            builder.Property(e => e.NrTamanhoBorda).HasColumnType("varchar(150)");
            builder.Ignore(e => e.ValidationResult);
            builder.ToTable("DocumentoConfiguracao");
            builder.Ignore(e => e.DocumentoItem);
        }
    }
}
