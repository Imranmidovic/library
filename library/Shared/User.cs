using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.Shared
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get => $"{Name} {LastName}"; }
        public bool LoggedIn { get; set; }

        [NotMapped]
        public Dictionary<string, string> Claims { get; set; }
    }
}
