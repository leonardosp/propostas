using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Documentos;
using ViverBem.Infra.Data.Extensions;

namespace ViverBem.Infra.Data.Mappings
{
    public class DocumentoConfiguracaoItemMapping : EntityTypeConfiguration<DocumentoConfiguracaoItem>
    {
        public override void Map(EntityTypeBuilder<DocumentoConfiguracaoItem> builder)
        {
            builder.Property(e => e.NoFonte).HasColumnType("varchar(max)");
            builder.Property(e => e.NoFonteColor).HasColumnType("varchar(150)");
            builder.Property(e => e.NoFrase).HasColumnType("varchar(150)");
            builder.Property(e => e.NoTipoFonte).HasColumnType("varchar(150)");
            builder.Property(e => e.NrColuna).HasColumnType("varchar(150)");
            builder.Property(e => e.NrFonteTamanho).HasColumnType("varchar(150)");
            builder.Property(e => e.NrMaxImageHeight).HasColumnType("varchar(150)");
            builder.Property(e => e.NrMaxImageWidth).HasColumnType("varchar(150)");
            builder.Ignore(e => e.ValidationResult);
            builder.ToTable("DocumentoConfiguracaoItem");
            builder.HasOne(e => e.DocConfiguracao).WithMany(o => o.GetDocumentoConfiguracaoItems).HasForeignKey(e => e.NrSeqDocumentoConfiguracao);

        }
    }
}
