using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core_Project_Arefin.Models;

namespace Core_Project_Arefin.ViewModels
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Cliams = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }
    }
}
