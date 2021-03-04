using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Models
{
    public class SysUser
    {
        [Key]
        public string id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string username { get; set; }
        [Column(TypeName = "varchar(20)")]

        public string nickname { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string password { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string email { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string phone { get; set; }
        public DateTime? birthday { get; set; }
        public DateTime? register_time { get; set; }
        public DateTime? last_login_time { get; set; }
    }
}
