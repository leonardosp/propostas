using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;
using ViverBem.Domain.Clientes.Commands;

namespace ViverBem.Domain.Combos.Commands
{
    public class AtualizarComboCommand : Command
    {
        public Guid ComboId { get; private set; }
        public string CodCombo { get; private set; }

        public string PlanoSrv { get; private set; }

        public String PlaPrinServ { get; private set; }
        public Guid? comboPrincipalId { get; private set; }

        public string VlrPremio { get; private set; }

        public string VlrCapital { get; private set; }

        public string CodComissUsr { get; private set; }
        public AtualizarComboCommand(string codcombo, string planosrv, Guid comboId, Guid? comoboPrincipalId, string plaprinserv, string vlrpremio, string vlrcapital, string codcomissusr)
        {
            ComboId = comboId;
            comboPrincipalId = comoboPrincipalId;
            CodCombo = codcombo;
            PlanoSrv = planosrv;
            PlaPrinServ = plaprinserv;
            VlrPremio = vlrpremio;
            VlrCapital = vlrcapital;
            CodComissUsr = codcomissusr;
        }
    }
}
