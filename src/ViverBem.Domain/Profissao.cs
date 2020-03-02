using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain
{
    public class Profissao
    {
        [Key]
        public int Id { get; set; }
        public int profcod { get; set; }

        public string profdesc { get; set; }
    }
}
