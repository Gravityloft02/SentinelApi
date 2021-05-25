using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SentinelApi.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string SchoolCode { get; set; }
        public string RollNo { get; set; }
    }
}
