using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ViverBem.Domain.Documentos;
using ViverBem.Domain.Documentos.Repository;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.Repository
{
    public class DocumentoConfiguracaoRepository : Repository<DocumentoConfiguracao>, IDocumentoConfiguracaoRepository
    {
        public DocumentoConfiguracaoRepository(ClientesContext context) : base(context)
        {

        }
        public override IEnumerable<DocumentoConfiguracao> ObterTodos()
        {
            var sql = "SELECT * FROM DOCUMENTOCONFIGURACAO D " +
                "WHERE D.EXCLUIDO = 0 ";

            return Db.Database.GetDbConnection().Query<DocumentoConfiguracao>(sql);
        }
        public void AdicionarDocumentoItem(DocumentoConfiguracaoItem documentoItem)
        {
            Db.DocumentoConfiguracaoItem.Add(documentoItem);
        }

        public void AtualizarDocumentoItem(DocumentoConfiguracaoItem documentoItem)
        {
            Db.DocumentoConfiguracaoItem.Update(documentoItem);
        }

        public IEnumerable<DocumentoConfiguracao> ObterDocumento(Guid organizadorId)
        {
            var sql = "SELECT * FROM DOCUMENTOCONFIGURACAO D " +
                            "WHERE D.EXCLUIDO = 0 ";

            return Db.Database.GetDbConnection().Query<DocumentoConfiguracao>(sql);
        }

        public IEnumerable<DocumentoConfiguracaoItem> ObterDocumentoItemPorDocumento(Guid documentoId)
        {
            var sql = @"SELECT * FROM DOCUMENTOCONFIGURACAOITEM D " +
                            "WHERE D.NRSEQDOCUMENTOCONFIGURACAO = @uid";

            return Db.Database.GetDbConnection().Query<DocumentoConfiguracaoItem>(sql, new { uid = documentoId });

        }

        public override DocumentoConfiguracao ObterPorId(Guid id)
        {
            var sql = @"SELECT * FROM DOCUMENTOCONFIGURACAO C " +
                         "WHERE C.NRSEQDOCUMENTOCONFIGURACAO = @uid";

            var doc = Db.Database.GetDbConnection().Query<DocumentoConfiguracao>(sql, new { uid = id });
            return doc.SingleOrDefault();
        }
        public DocumentoConfiguracaoItem ObterDocumentoItemPorId(Guid id)
        {
            var sql = @"SELECT * FROM DOCUMENTOCONFIGURACAOITEM D " +
                                "WHERE D.NRSEQDOCUMENTOCONFIGURACAOITEM = @uid";

            var docItem = Db.Database.GetDbConnection().Query<DocumentoConfiguracaoItem>(sql, new { uid = id });
            return docItem.SingleOrDefault();
        }
        public override void Remover(Guid id)
        {
            var documento = ObterPorId(id);
            documento.ExcluirDocumento();
            Atualizar(documento);
        }
    }
}
