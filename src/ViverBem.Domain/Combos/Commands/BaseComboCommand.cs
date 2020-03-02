using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Combos.Commands
{
    public abstract class BaseComboCommand : Command
    {
        public Guid Id { get; protected set; }
        public string CodCombo { get; protected set; }

        public string PlanoSrv { get; protected set; }

        public String PlaPrinServ { get; protected set; }

        public string VlrPremio { get; protected set; }

        public string VlrCapital { get; protected set; }

        public string CodComissUsr { get; protected set; }
    }
}
