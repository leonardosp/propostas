using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Domain
{
    public class DebitoEmConta
    {
        [Key]
        public int Id { get; set; }
        public int bancod { get; set; }

        public string bannome { get; set; }
    }
}
