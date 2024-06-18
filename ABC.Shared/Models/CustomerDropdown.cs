using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Models
{
    public class CustomerDropdown: IdentityUser
    {
        public String Id { get; set; }
        public string Name {  get; set; }
        public string? City { get; set; }
        public int ContactNumber { get; set; }

    }
}
