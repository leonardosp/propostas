using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class GrauParentescoViewModel
    {
        public string GrauCod { get; set; }
        public string GrauDesc { get; set; }

        public static List<GrauParentescoViewModel> ListarGrausParentesco()
        {
            return new List<GrauParentescoViewModel>()
            {
                new GrauParentescoViewModel() {GrauCod="1", GrauDesc="Filho"},
                new GrauParentescoViewModel() {GrauCod="2", GrauDesc="Conjuge"},
                new GrauParentescoViewModel() {GrauCod="3", GrauDesc="Companheiro(a)"},
                new GrauParentescoViewModel() {GrauCod="4", GrauDesc="Irmão(a)"},
                new GrauParentescoViewModel() {GrauCod="5", GrauDesc="Pai"},
                new GrauParentescoViewModel() {GrauCod="6", GrauDesc="Mãe"},
                new GrauParentescoViewModel() {GrauCod="7", GrauDesc="Sobrinho(a)"},
                new GrauParentescoViewModel() {GrauCod="8", GrauDesc="Primo(a)"},
                new GrauParentescoViewModel() {GrauCod="9", GrauDesc="Cunhado(a)"},
                new GrauParentescoViewModel() {GrauCod="10", GrauDesc="Sogro(a)"},
                new GrauParentescoViewModel() {GrauCod="11", GrauDesc="Genro(Nora)"},
                new GrauParentescoViewModel() {GrauCod="12", GrauDesc="Tio(a)"},
                new GrauParentescoViewModel() {GrauCod="13", GrauDesc="Sem Parentesco"},
                new GrauParentescoViewModel() {GrauCod="14", GrauDesc="Neto(a)"},
                new GrauParentescoViewModel() {GrauCod="15", GrauDesc="Outros"},
                new GrauParentescoViewModel() {GrauCod="16", GrauDesc="Dep Menor"},
                new GrauParentescoViewModel() {GrauCod="17", GrauDesc="Dep Univerc"},
                new GrauParentescoViewModel() {GrauCod="18", GrauDesc="Noivo(a)"},
                new GrauParentescoViewModel() {GrauCod="19", GrauDesc="Filho(a) Invalido(a)"},
                new GrauParentescoViewModel() {GrauCod="20", GrauDesc="Enteado(a)"},
                new GrauParentescoViewModel() {GrauCod="21", GrauDesc="Avo(a)"},
                new GrauParentescoViewModel() {GrauCod="22", GrauDesc="Bineto(a)"}
            };
        }
    }
}
