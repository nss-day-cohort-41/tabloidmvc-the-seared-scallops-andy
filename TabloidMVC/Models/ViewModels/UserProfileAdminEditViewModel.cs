using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class UserProfileAdminEditViewModel
    {
        public UserProfile Profile { get; set; }
        public List<UserType> Types { get; set; }
    }
}
