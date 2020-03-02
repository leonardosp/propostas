using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class DocumentoConfiguracaoViewModel
    {
        public DocumentoConfiguracaoViewModel()
        {
            NrSeqDocumentoConfiguracao = Guid.NewGuid();
            DocumentoConfiguracaoItem = new DocumentoConfiguracaoItemViewModel();
        }
       
        [Key]
        public Guid NrSeqDocumentoConfiguracao { get;  set; }

        [Display(Name ="Nome do Documento")]
        public string CdDocumentoConfiguracao { get;  set; }

        [Display(Name ="Numero de ordem")]
        [Required(ErrorMessage = "A ordem é requerida")]
        public Int32 NrOrdem { get;  set; }

        [Display(Name ="Numero da Coluna")]
        [Required(ErrorMessage = "O numero da coluna é requerido")]
        public Int32 NrColuna { get;  set; }

        [Display(Name ="Cor de fundo do Contrato")]
        public string NoBackGroundColor { get;  set; }

        [Display(Name ="Alinhamento da Coluna")]
        public string NoHorizontalAligment { get;  set; }

        [Display(Name ="Tamanho da borda da Coluna")]
        public Int32 NrTamanhoBorda { get;  set; }

        [Display(Name ="Margem a esquerda da coluna")]
        public Int32 NrMarginLeft { get;  set; }

        public string NoPropriedadeLista { get;  set; }

        public int NrParagraphspacing { get;  set; }

        public int NrWidth { get;  set; }

        public TipoDocumentoViewModel TipoDocumento { get; set; }
        public TipoAlinhamentoViewModel TipoAlinhamentoDoc { get; set; }
        public CorFundoViewModel FundoDoc { get; set; }
        public DocumentoConfiguracaoItemViewModel DocumentoConfiguracaoItem { get; set; }
        public IEnumerable<DocumentoConfiguracaoItemViewModel> DocumentoConfiguracaoItems { get; set; }

        public SelectList Documentos()
        {
            return new SelectList(TipoDocumentoViewModel.ListarDocumentos(), "TipoDoc", "NomeDoc");
        }
        public SelectList Alinhamento()
        {
            return new SelectList(TipoAlinhamentoViewModel.ListarAlinhamentos(), "TipoAlinhamento", "NomeAlinhamento");
        }
        public SelectList Fundo()
        {
            return new SelectList(CorFundoViewModel.ListarAlinhamentos(), "TipoAlinhamento", "NomeAlinhamento");
        }
    }
}
