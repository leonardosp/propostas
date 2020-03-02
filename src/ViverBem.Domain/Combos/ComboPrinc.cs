using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Combos
{
    public class ComboPrinc : Entity<ComboPrinc>
    {
        public ComboPrinc(string codcombo, string corretor,Guid combprincid)
        {
            Id = Guid.NewGuid();
            CodCombo = codcombo;
            Corretor = corretor;
            comboPrincId = combprincid;
        }
  


        private ComboPrinc()
        {

        }
        public virtual ICollection<Combo> GetCombos { get; private set; }
        public virtual Combo Combo { get; private set; }
        public String Corretor { get; private set; }

        public string CodCombo { get; private set; }
        public Guid comboPrincId { get; private set; }

        public override bool EhValido()
        {
            return true;
        }

        public static class ComboPrincFactory
        {
            public static ComboPrinc NovoComboCompleto(Guid id, string corretor, string codcombo)
            {
                var combo = new ComboPrinc()
                {
                    Id = id,
                    CodCombo = codcombo,
                    Corretor = corretor
                };

                return combo;
            }
        }
    }
}
