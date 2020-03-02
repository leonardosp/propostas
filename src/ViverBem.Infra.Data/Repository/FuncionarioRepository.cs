using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Funcionarios;
using ViverBem.Domain.Funcionarios.Repository;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.Repository
{
    public class FuncionarioRepository : Repository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(ClientesContext context) : base(context)
        {

        }
    }
}
