using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;

namespace ViverBem.Application.Interfaces
{
    public interface ITokenAppService : IDisposable
    {
        void Registrar(TokenResultViewModel tokenViewModel);
        IEnumerable<TokenResultViewModel> ObterTodos();
        TokenResultViewModel ObterPorId(Guid id);
        void Atualizar(TokenResultViewModel clienteViewModel);
        void Excluir(Guid id);

    }
}
