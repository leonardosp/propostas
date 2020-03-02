using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Combos
{
    public class Combo : Entity<Combo>
    {
        public Combo(string codcombo, string planosrv, string plaprinserv, string vlrpremio, string vlrcapital, string codcomissusr, Guid? comboPrincId)
        {
            Id = Guid.NewGuid();
            CodCombo = codcombo;
            PlanoSrv = planosrv;
            PlaPrinServ = plaprinserv;
            VlrPremio = vlrpremio;
            VlrCapital = vlrcapital;
            CodComissUsr = codcomissusr;
            ComboPrincId = comboPrincId;
        }

        private Combo()
        {

        }
        public string CodCombo { get; private set; }

        public string PlanoSrv { get; private set; }

        public String PlaPrinServ { get; private set; }

        public string VlrPremio { get; private set; }

        public string VlrCapital { get; private set; }
        public virtual ComboPrinc ComboPrinc { get; private set; }
        public Guid? ComboPrincId { get; private set; }
        public string CodComissUsr { get; private set; }
        public bool Excluido { get; private set; }

        public void ExcluirComboItem()
        {
            Excluido = true;
        }
        public override bool EhValido()
        {
            return true;
        }

        public static class ComboFactory
        {
            public static Combo NovoComboCompleto(Guid id,string codcombo, string planosrv, string plaprinserv, string vlrpremio, string vlrcapital, string codcomissusr, Guid? comboprincId)
            {
                var combo = new Combo()
                {
                    Id = id,
                    CodCombo = codcombo,
                    PlanoSrv = planosrv,
                    PlaPrinServ = plaprinserv,
                    VlrPremio = vlrpremio,
                    VlrCapital = vlrcapital,
                    CodComissUsr = codcomissusr,
                    ComboPrincId = comboprincId
                };

                return combo;
            }
        }
    }
}
