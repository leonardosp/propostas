using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Combos.Repository
{
    public interface IComboRepository : IRepository<ComboPrinc>
    {
        IEnumerable<ComboPrinc> ObterComboPrinc(Guid comboId);
        IEnumerable<Combo> ObterComboItemPorComboPrinc(Guid comboitemId);
        Combo ObtercomboItemItemPorId(Guid id);
        void AdicionarComboItem(Combo comboItem);
        void AtualizarComboItem(Combo comboItem);
    }
}
