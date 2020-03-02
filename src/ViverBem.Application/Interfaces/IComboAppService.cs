using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;

namespace ViverBem.Application.Interfaces
{
    public interface IComboAppService : IDisposable
    {
        void Registrar(ComboPrincViewModel documentoConfiguracao);
        IEnumerable<ComboPrincViewModel> ObterTodos();
        ComboPrincViewModel ObterPorId(Guid id);
        void Atualizar(ComboPrincViewModel comboPrinc);
        void Excluir(Guid id);
        void AdicionarComboItem(ComboViewModel comboViewModel);
        void AtualizarDocumentoItem(ComboViewModel comboViewModel);
        IEnumerable<ComboViewModel> ObterPorComboPrinc(Guid comboId);
    }
}
