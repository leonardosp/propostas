using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViverBem.Domain;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Propostas;
using ViverBem.Domain.Propostas.Repository;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.Repository
{
    public class PropostaRepository : Repository<Propostas>, IPropostasRepository
    {
        public PropostaRepository(ClientesContext context) : base(context)
        {

        }
        public IEnumerable<Cliente> ObterClientes()
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                "INNER JOIN ENDERECOS EN " +
                "ON C.ID = EN.CLIENTEID " +
                "WHERE C.EXCLUIDO = 0 ";

            var evento = Db.Database.GetDbConnection().Query<Cliente, Endereco, Cliente>(sql,
                (c, en) =>
                {
                    if (en != null)
                        c.AtribuirEndereco(en);

                    return c;
                });

            return evento;
        }
        public override IEnumerable<Propostas> ObterTodos()
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
                "WHERE P.EXCLUIDO = 0 ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql);
        }

        public override Propostas ObterPorId(Guid id)
        {
            var sql = @"SELECT * FROM PROPOSTAS C " +
                    "WHERE C.ID = @uid";


            var proposta = Db.Database.GetDbConnection().Query<Propostas>(sql, new { uid = id });
            return proposta.SingleOrDefault();
        }

        public override void Remover(Guid id)
        {
            var cliente = ObterPorId(id);
            cliente.ExcluirCliente();
            Atualizar(cliente);
        }

        public IEnumerable<Propostas> ObterPropostaPorFuncionario(Guid organizadorId)
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
                     "WHERE P.FUNCIONARIOID = @oid ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql, new { oid = organizadorId });
        }

        public Cliente ObterClientePorid(Guid id)
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

        public IEnumerable<Dependente> ObterPorCliente(Guid clienteId)
        {
            var sql = @"SELECT * FROM DEPENDENTES D " +
                         "WHERE D.CLIENTEID = @oid ";

            return Db.Database.GetDbConnection().Query<Dependente>(sql, new { oid = clienteId });
        }

        public IEnumerable<ComboPrinc> ObterCombos()
        {
            var sql = "SELECT * FROM COMBOSPRINC";

            return Db.Database.GetDbConnection().Query<ComboPrinc>(sql);
        }

        public IEnumerable<Combo> ObterCombosPorId(Guid id)
        {
            var sql = @"SELECT * FROM COMBOS D " +
               "WHERE D.COMBOPRINCID = @uid";

            return Db.Database.GetDbConnection().Query<Combo>(sql, new { uid = id });
        }

        public DebitoEmConta ObterDebitoEmContaPorId(Guid id)
        {
            var sql = @"SELECT * FROM DEBITOEMCONTA D " +
                        "WHERE D.ID = @uid";

            var d = Db.Database.GetDbConnection().Query<DebitoEmConta>(sql, new { uid = id });
            return d.SingleOrDefault();
        }

        public IEnumerable<DebitoEmConta> ObterDebitoEmConta()
        {
            var sql = "SELECT * FROM DEBITOEMCONTA";

            return Db.Database.GetDbConnection().Query<DebitoEmConta>(sql);
        }

        public Cliente ObterClientePorNome(string nome)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                     "WHERE C.NOME = @nome";

            var c = Db.Database.GetDbConnection().Query<Cliente>(sql, new { nome = nome });
            return c.SingleOrDefault();
        }

        public IEnumerable<Propostas> ObterPropostasRecusadas()
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
                             "WHERE P.EXCLUIDO = 1 ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql);
        }

        public IEnumerable<Propostas> ObterPropostasAceitas()
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
                            "WHERE P.APROVADO = 1 " +
                            "AND P.EXCLUIDO = 0";

            return Db.Database.GetDbConnection().Query<Propostas>(sql);
        }

        public IEnumerable<Propostas> ObterPropostasParaAprovar()
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
                "WHERE P.APROVADO = 0 " +
                "AND P.EXCLUIDO = 0 ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql);
        }

        public IEnumerable<Propostas> ObterPropostasPorData(DateTime DtInic, DateTime DtFin)
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
                        "WHERE P.DATACADASTRO  BETWEEN @ini " +
                        "AND @fim ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql, new { ini = DtInic, fim = DtFin });
        }

        public IEnumerable<Propostas> ObterPropostasAprovadasPorData(DateTime DtInic, DateTime DtFin)
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
            "WHERE P.APROVADO = 1 " +
            "AND P.DataCadastro  BETWEEN @ini " +
            "AND @fim ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql, new { ini = DtInic, fim = DtFin });
        }

        public IEnumerable<Propostas> ObterPropostasRecusadasPorData(DateTime DtInic, DateTime DtFin)
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
           "WHERE P.EXCLUIDO = 1 " +
           "AND P.DataCadastro  BETWEEN @ini " +
           "AND @fim ";

            return Db.Database.GetDbConnection().Query<Propostas>(sql, new { ini = DtInic, fim = DtFin });
        }

        public Propostas ObterUltimaProposta()
        {
            var sql = @"SELECT * FROM PROPOSTAS C " +
                     "WHERE DATACADASTRO IS NOT NULL " +
                     "AND STATUSFINANCEIRO = (select max(STATUSFINANCEIRO) from PROPOSTAS)";

            var c = Db.Database.GetDbConnection().Query<Propostas>(sql);
            return c.SingleOrDefault();
        }

        public IEnumerable<Cliente> ObterClientesPorFuncionario(Guid funcionarioId)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
              "WHERE C.EXCLUIDO = 0 " +
              "AND C.FUNCIONARIOID = @oid " +
              "ORDER BY C.CPF DESC";

            return Db.Database.GetDbConnection().Query<Cliente>(sql, new { oid = funcionarioId });
        }

        public Cliente ObterClientePorCpf(string Cpf)
        {
            var sql = @"SELECT * FROM CLIENTES C " +
                     "LEFT JOIN ENDERECOS EN " +
                        "ON C.ID = EN.CLIENTEID " +
                        "WHERE C.CPF = @uid";

            var evento = Db.Database.GetDbConnection().Query<Cliente, Endereco, Cliente>(sql,
                (c, en) =>
                {
                    if (en != null)
                        c.AtribuirEndereco(en);

                    return c;
                }, new { uid = Cpf });

            return evento.FirstOrDefault();
        }

        public IEnumerable<Propostas> ObterPropostasComPronum()
        {
            var sql = @"SELECT * FROM PROPOSTAS P " +
             "WHERE P.PRONUM  IS NOT NULL";

            return Db.Database.GetDbConnection().Query<Propostas>(sql);
        }

        public Propostas ObterPropostaPorCpf(string cpf)
        {
            var sql = @"SELECT * FROM PROPOSTAS D " +
                         "WHERE D.CPF = @cpf ";

            var c = Db.Database.GetDbConnection().Query<Propostas>(sql, new { cpf = cpf });
            return c.SingleOrDefault();
        }
    }
}
