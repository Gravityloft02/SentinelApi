using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SentinelApi.Models
{
    public class Registermodel
    {
        [Required(ErrorMessage ="UserName is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "SchoolCode is Required")]
        public string SchoolCode { get; set; }

        [Required(ErrorMessage = "RollNo is Required")]
        public string RollNo { get; set; }
    }
}
