using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vincall.OauthService.Models
{
    public class LoginViewModel : LoginInputModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;      

    }
}
