using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Glow.Models
{
    public class Sendmail
    {

            [Required]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.MultilineText)]
            public string Detail { get; set; }
            [Required]
            public string Fullname { get; set; }
            [Required]
            public string MobilNo { get; set; }

            [Required]
            public string subject { get; set; }


       
    }
}
