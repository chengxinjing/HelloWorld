using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWeb.Models
{
    [SerializableAttribute]
    public class AdminUser
    {
        private string _UserId { get; set; }
        private string _RoleId { get; set; }
        private string _Password { get; set; }
        private string _salt { get; set; }
        
    }
}