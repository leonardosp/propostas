using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Combos.Events
{
    public class ComboAtualizadoEvent : Event
    {
        public string CodCombo { get; private set; }

        public string PlanoSrv { get; private set; }

        public String PlaPrinServ { get; private set; }

        public string VlrPremio { get; private set; }

        public string VlrCapital { get; private set; }
        public virtual ComboPrinc ComboPrinc { get; private set; }
        public Guid? ComboPrincId { get; private set; }
        public string CodComissUsr { get; private set; }
        public Guid ComboId { get; set; }

        public ComboAtualizadoEvent(string codcombo, string planosrv, string plaprinserv, string vlrpremio, string vlrcapital, Guid? comboprincid, string codcomissuusr, Guid comboid)
        {
            CodCombo = codcombo;
            PlanoSrv = planosrv;
            PlaPrinServ = plaprinserv;
            VlrPremio = vlrpremio;
            VlrCapital = vlrcapital;
            ComboPrincId = comboprincid;
            ComboId = comboid;
        }
    }
}
