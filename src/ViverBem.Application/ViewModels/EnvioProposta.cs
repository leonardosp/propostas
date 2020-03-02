using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class EnvioProposta
    {
        public string corretor { get; set; }
        public string cpf { get; set; }
        public string nome { get; set; }
        public string dt_nasc { get; set; }
        public string sexo { get; set; }
        public string est_civil { get; set; }

        public string ender { get; set; }
        public string endnum { get; set; }
        public string endcompl { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string dddtel { get; set; }
        public string tel { get; set; }
        public string dddcel { get; set; }
        public string cel { get; set; }
        public string identidade { get; set; }
        public string org_emissor { get; set; }
        public string email { get; set; }
 
        public string cod_prof { get; set; }


        public string ocupacao { get; set; }

        public string dt_venda { get; set; }
        public string id_ext_props { get; set; }

        public string cod_combo { get; set; }

        public string vl_premio { get; set; }

        public string vl_capital { get; set; }
        public string tp_pgto { get; set; }
        public string banco { get; set; }
        public string agencia { get; set; }

        public string agencia_dv { get; set; }
        public string cc { get; set; }
        public string cc_dv { get; set; }

        public string dia_debito { get; set; }

        public string tipo_envio { get; set; }

        public string endercobr { get; set; }

        public string endnumcobr { get; set; }

        public string endcomplcobr { get; set; }

        public string bairrocobr { get; set; }

        public string cidadecobr { get; set; }

        public string ufcobr { get; set; }
        public string cepcobr { get; set; }
        public string emailcobr { get; set; }
        public string ppe { get; set; }
        public List<DependenteProposta> benef { get; set; }
    }
}
