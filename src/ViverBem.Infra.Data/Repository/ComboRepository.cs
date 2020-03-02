using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Combos.Repository;
using ViverBem.Domain.Interfaces;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.Repository
{
    public class ComboRepository : Repository<ComboPrinc>, IComboRepository
    {
        public ComboRepository(ClientesContext context) : base(context)
        {

        }
        public override IEnumerable<ComboPrinc> ObterTodos()
        {
            var sql = "SELECT * FROM COMBOSPRINC";

            return Db.Database.GetDbConnection().Query<ComboPrinc>(sql);
        }
        public void AdicionarComboItem(Combo comboItem)
        {
            Db.Combos.Add(comboItem);
        }
        public override ComboPrinc ObterPorId(Guid id)
        {
            var sql = @"SELECT * FROM COMBOSPRINC D " +
                "WHERE D.COMBOPRINCID = @oid ";

            var combo = Db.Database.GetDbConnection().Query<ComboPrinc>(sql, new { oid = id });
            return combo.SingleOrDefault();
        }
        public Combo ObtercomboItemItemPorId(Guid id)
        {
            var sql = @"SELECT * FROM COMBOS D " +
                    "WHERE D.COMBOID = @uid";

            var docItem = Db.Database.GetDbConnection().Query<Combo>(sql, new { uid = id });
            return docItem.SingleOrDefault();
        }

        public IEnumerable<Combo> ObterComboItemPorComboPrinc(Guid comboitemId)
        {
            var sql = @"SELECT * FROM COMBOS D " +
                            "WHERE D.COMBOPRINCID = @uid";

            return Db.Database.GetDbConnection().Query<Combo>(sql, new { uid = comboitemId });
        }

        public IEnumerable<ComboPrinc> ObterComboPrinc(Guid comboId)
        {
            var sql = @"SELECT * FROM COMBOSPRINC C " +
                        "WHERE C.ID = @uid";

            return Db.Database.GetDbConnection().Query<ComboPrinc>(sql, new { uid = comboId });
        }

        public void AtualizarComboItem(Combo comboItem)
        {
            Db.Combos.Update(comboItem);
        }
    }
}
