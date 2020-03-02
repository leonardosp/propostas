using Celebre.FrameWork.Seg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class Menu : UserControl
    {
        TransacaoS listTransacoes = new TransacaoS();
        MenuS listMenus = new MenuS();
        PermissaoS permissoes = new PermissaoS();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.GenerateMenu();
        }

        /// <summary>
        ///     Método Utilizado para gerar o menu
        /// </summary>
        /// <author>Eduardo Mello</author>
        /// <date>11/05/2012</date>
        private void GenerateMenu()
        {
            //Cria Div com classe menu
            HtmlGenericControl menuControl = new HtmlGenericControl("div");
            menuControl.Attributes.Add("class", "col-xs-2");
            menuControl.Attributes.Add("id", "sidebar-left");

            //Menu Search
            menuControl.InnerHtml = string.Format("<div class='menu-search'><input type='text' autocomplete='off' id='txtSearchMenu' name='txtSearchMenu' style='width: 85%;' placeholder='digite para pesquisar' /><i class='fas fa-search'></i></div>");

            //Pesquisa todos os menus para o grupo empresa
            listMenus.NrSeqGrupoempresa = Sessao.GetInstance().NrSeqGrupoEmpresa;
            listMenus.Localizar();

            //Pesquisa todas as transações para o grupo empresa
            listTransacoes.NrSeqGrupoEmpresa = Sessao.GetInstance().NrSeqGrupoEmpresa;
            listTransacoes.Localizar();

            //Percorre todos os perfis dos usuarios
            foreach (UsuarioPerfil perfil in Sessao.GetInstance().Usuario.UsuarioPerfilS)
            {
                //Pesquisa as permissoes do perfil
                PermissaoS permissoesAux = new PermissaoS();
                permissoesAux.NrSeqGrupoEmpresa = Sessao.GetInstance().NrSeqGrupoEmpresa;
                permissoesAux.NrSeqPerfil = perfil.NrSeqPerfil;
                permissoesAux.Localizar();

                //adiciona as permissoes a lista de permissoes
                permissoes.AddRange(permissoesAux);
            }

            //Carrega o menu dentro do controle div
            menuControl.InnerHtml += string.Format("<div class='sidebar-nav nav-collapse navbar-collapse'><ul class='nav main-menu'>{0}</ul></div>",
                createMenuItem(listMenus.Where(p => p.NrSeqMenupai == Int32.MinValue).OrderBy(p => p.NrOrdem).ToList()));

            //adiciona a div ao controle
            this.Controls.Add(menuControl);
        }

        /// <summary>
        ///     Método Recursivo Utilizado para Adicionar Menus, e Transações
        /// </summary>
        /// <param name="menu">Objeto Menu</param>
        /// <param name="HasChildren">Boleano que indica se tem filhos</param>
        /// <returns>Retorna string com o HTML dos menus e transações</returns>
        /// <author>Eduardo Mello</author>
        /// <date>11/05/2012</date>
        private string createMenuItem(Celebre.FrameWork.Seg.Menu menu, bool HasChildren)
        {
            //verifica se tem filho
            if (HasChildren)
            {
                //retorna os submenus do menu
                string menusFilhos = createMenuItem(listMenus.Where(p => p.NrSeqMenupai == menu.NrSeqMenu).OrderBy(p => p.NrOrdem).ToList());
                //retorna as Transacoes do menu
                string transacoesFilhas = createTransacaoItem(listTransacoes.Where(p => p.NrSeqMenu == menu.NrSeqMenu).OrderBy(p => p.NrOrdem).ToList(), menu.NrNivel);

                //Verifica se o menu tem alguma transação ou sub-menu 
                if (!string.IsNullOrEmpty(menusFilhos) || !string.IsNullOrEmpty(transacoesFilhas))
                {
                    //adiciona o menu seus sub-menus e suas transações
                    return string.Format("<li><a href='#' class='dropmenu row' data-submenu-nivel='{0}'><span class='col-xs-2 vcenter'><i class='{1}'></i></span><span class='hidden-xs text col-xs-8 vcenter'>{2}</span><span class='chevron closed col-xs-1 vcenter'></span></a><ul>{3}{4}</ul></li>",
                        menu.NrNivel,
                        !string.IsNullOrEmpty(menu.CSSImagem) ? "fas " + menu.CSSImagem : "far fa-folder-open",
                        menu.NoMenu,
                        menusFilhos,
                        transacoesFilhas
                    );
                }
                else
                    return string.Empty;
            }
            else
            {
                //retorna as Transacoes do menu
                string transacoesFilhas = createTransacaoItem(listTransacoes.Where(p => p.NrSeqMenu == menu.NrSeqMenu).OrderBy(p => p.NrOrdem).ToList(), menu.NrNivel);

                //Verifica se o menu tem alguma transação
                if (!string.IsNullOrEmpty(transacoesFilhas))
                {
                    //adiciona o menu e suas transações
                    return string.Format("<li><a href='#' class='dropmenu row' data-submenu-nivel='{0}'><span class='col-xs-2 vcenter'><i class='fas {1}'></i></span><span class='hidden-xs text col-xs-8 vcenter'>{2}</span></span><span class='chevron closed col-xs-1 vcenter'></span></a><ul>{3}</ul></li>",
                        menu.NrNivel,
                        !string.IsNullOrEmpty(menu.CSSImagem) ? "fas " + menu.CSSImagem : "far fa-folder-open",
                        menu.NoMenu,
                        transacoesFilhas);
                }
                else
                    return string.Empty;
            }
        }

        /// <summary>
        ///     Método utilizado para adicionar Transações 
        /// </summary>
        /// <param name="transacoes">Lista de Transacoes do menu</param>
        /// <param name="nrNivelPai">Nivel do menu pai</param>
        /// <returns>Retorna string com o HTML das transações adicionadas</returns>
        /// <author>Eduardo Mello</author>
        /// <date>11/05/2012</date>
        private string createTransacaoItem(List<Transacao> transacoes, int nrNivelPai)
        {
            string transacoesItems = string.Empty;

            //Percorre todas as transações passadas por parametro
            foreach (Transacao transacao in transacoes)
            {
                //Adiciona o código JS para abrir a tab dependendo do parametro do banco (comentário acima)

                string jsOpenTable = "";

                if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("ApplicationSubPath"))
                {
                    jsOpenTable = string.Format("javascript:$(\"#celebreMasterTabs\").celebreTabs(\"addWithInner\",\"{0}\", \"../Sistema/{1}/{2}\",\"{3}\",\"SELPAGE\",\"Seleção\")", transacao.NoTransacao, transacao.NoDirFonte, transacao.TransacaoPaginaPrincipal.NoPaginaweb, System.Configuration.ConfigurationManager.AppSettings["ApplicationSubPath"]);
                }
                else
                {
                    jsOpenTable = string.Format("javascript:$(\"#celebreMasterTabs\").celebreTabs(\"addWithInner\",\"{0}\", \"../Sistema/{1}/{2}\",\"{3}\",\"SELPAGE\",\"Seleção\")", transacao.NoTransacao, transacao.NoDirFonte, transacao.TransacaoPaginaPrincipal.NoPaginaweb, "");
                }
               

                //Verifica se o usuario tem a permissão para acessar a transação em questão
                if (permissoes.Where(p => p.NrSeqTransacao == transacao.NrSeqTransacao).Count() > 0)
                {
                    //Adiciona as transações para o menu
                    transacoesItems += string.Format("<li><a class='submenu row' data-submenu-nivel='{0}' href='{1}'><span class='col-xs-2 vcenter'><i class='fas fa-asterisk'></i></i></span><span class='hidden-xs text col-xs-10 vcenter'>{2}</span></a></li>",
                        nrNivelPai +1,
                        jsOpenTable,
                        transacao.NoTransacao);
                }
            }

            return transacoesItems;
        }

        /// <summary>
        ///     Método utilizado para Criar os Menus
        /// </summary>
        /// <param name="listmenu">Lista de Menus Pai (Nivel 1)</param>
        /// <returns>Retorna string HTML com os menus e transações</returns>
        /// <author>Eduardo Mello</author>
        /// <date>11/05/2012</date>
        private string createMenuItem(List<Celebre.FrameWork.Seg.Menu> listmenu)
        {
            string htmlReturn = string.Empty;

            //Percorre todos menus Pais             
            foreach (Celebre.FrameWork.Seg.Menu item in listmenu)
            {
                //Seleciona todos Menus Filho 
                List<Celebre.FrameWork.Seg.Menu> menusChildren = listMenus.Where(p => p.NrSeqMenupai == item.NrSeqMenu).OrderBy(p => p.NrOrdem).ToList();

                //Cria o Menu
                htmlReturn += createMenuItem(item, (menusChildren.Count() > 0));
            }

            return htmlReturn;
        }
    }
}
