using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Combos.Events
{
    public class BaseComboEvent : Event
    {
        public string CodCombo { get; protected set; }

        public string PlanoSrv { get; protected set; }

        public String PlaPrinServ { get; protected set; }

        public string VlrPremio { get; protected set; }

        public string VlrCapital { get; protected set; }
        public virtual ComboPrinc ComboPrinc { get; protected set; }
        public Guid? ComboPrincId { get; protected set; }
        public string CodComissUsr { get; protected set; }
        public Guid ComboId { get; protected set; }
    }
}
