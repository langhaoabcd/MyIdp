using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIdp.Models.Ids4Model.Ids4
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        public bool Cancle { get; set; }
    }

    public class LoginSmsInput
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Code { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        public bool Cancle { get; set; }
    }
}
