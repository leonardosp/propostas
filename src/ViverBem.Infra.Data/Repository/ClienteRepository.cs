using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ViverBem.Domain;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Clientes.Repository;
using ViverBem.Domain.Combos;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.Repository
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ClientesContext context) : base(context)
        {

        }

        public override IEnumerable<Cliente> ObterTodos()
        {
            var sql = "SELECT * FROM CLIENTES C " +
                "WHERE C.EXCLUIDO = 0 ";

            return Db.Database.GetDbConnection().Query<Cliente>(sql);
        }
        public void AdicionarEndereco(Endereco endereco)
        {
            Db.Enderecos.Add(endereco);
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            Db.Enderecos.Update(endereco);
        }

        public Endereco ObterEnderecoPorId(Guid id)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                "WHERE C.ID = @uid";

            var endereco = Db.Database.GetDbConnection().Query<Endereco>(sql, new { uid = id });
            return endereco.SingleOrDefault();
        }

        public IEnumerable<Cliente> ObterClientePorFuncionario(Guid funcionarioId)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                  "WHERE C.EXCLUIDO = 0 " +
                  "AND C.FUNCIONARIOID = @oid " +
                  "ORDER BY C.CPF DESC";

            return Db.Database.GetDbConnection().Query<Cliente>(sql, new { oid = funcionarioId });
        }

        public override Cliente ObterPorId(Guid id)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                "LEFT JOIN ENDERECOS EN " +
                "ON C.ID = EN.CLIENTEID " +
                "WHERE C.ID = @uid";


            var evento = Db.Database.GetDbConnection().Query<Cliente, Endereco, Cliente>(sql,
                (c, en) =>
                {
                    if (en != null)
                        c.AtribuirEndereco(en);

                    return c;
                }, new { uid = id });

            return evento.FirstOrDefault();
        }

        public override void Remover(Guid id)
        {
            var cliente = ObterPorId(id);
            cliente.ExcluirCliente();
            Atualizar(cliente);
        }

        public void AdicionarDependente(Dependente dependente)
        {
            Db.Dependentes.Add(dependente);
        }

        public void AtualizarDependente(Dependente dependente)
        {
            Db.Dependentes.Update(dependente);
        }

        public IEnumerable<Dependente> ObterDependentePorCliente(Guid clienteId)
        {
            var sql = @"SELECT * FROM DEPENDENTES D " +
                        "WHERE D.CLIENTEID = @oid ";  

            return Db.Database.GetDbConnection().Query<Dependente>(sql, new { oid = clienteId });
        }

        public IEnumerable<ComboPrinc> ObterCombos()
        {
            var sql = "SELECT * FROM COMBOSPRINC C ";

            return Db.Database.GetDbConnection().Query<ComboPrinc>(sql);
        }

        public Combo ObterComboSecundario(Guid id)
        {
            var sql = @"SELECT * FROM COMBOS D " +
            "WHERE D.COMBOPRINCID = @oid ";

            var combo= Db.Database.GetDbConnection().Query<Combo>(sql, new { oid = id });
            return combo.SingleOrDefault();
        }

        public IEnumerable<Profissao> ObterProfissoes()
        {
            var sql = "SELECT * FROM PROFISSAO C ";

            return Db.Database.GetDbConnection().Query<Profissao>(sql);
        }

        public Profissao ObterProfissaoPorId(Int32 id)
        {
            var sql = @"SELECT * FROM PROFISSAO D " +
                    "WHERE D.ID = @oid ";

            var combo = Db.Database.GetDbConnection().Query<Profissao>(sql, new { oid = id });
            return combo.SingleOrDefault();
        }

        public Cliente ObterPorCpf(string cpf)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                                "WHERE C.CPF = @uid";

            var endereco = Db.Database.GetDbConnection().Query<Cliente>(sql, new { uid = cpf });
            return endereco.FirstOrDefault();
        }
    }
}
