using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Models
{
    public class Graficos
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Name { get; set; }
        public int Value { get; set; }
    }
}