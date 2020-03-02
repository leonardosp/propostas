using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class DebitoEmContaDisponivel
    {
        [Key]
        public int Id { get; set; }
        public int bancod { get; set; }
        public string bannome { get; set; }

        public static List<DebitoEmContaDisponivel> ListarBancos()
        {
            return new List<DebitoEmContaDisponivel>()
            {
                new DebitoEmContaDisponivel() {bancod=33, bannome="SANTANDER BANESPA"},
                new DebitoEmContaDisponivel() {bancod=41, bannome="BANRISUL S/A"},
                new DebitoEmContaDisponivel() {bancod=104, bannome="CAIXA ECONOMICA FEDERAL"},
                new DebitoEmContaDisponivel() {bancod=237, bannome="BRADESCO S/A"},
                new DebitoEmContaDisponivel() {bancod=341, bannome="BANCO ITAU"},
                new DebitoEmContaDisponivel() {bancod=399, bannome="HSBC BANK BRASIL S/A"},
                new DebitoEmContaDisponivel() {bancod=748, bannome="SICREDI"},
            };
        }
    }
}
