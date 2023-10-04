using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_Ecommerce.Models
{
    public class AddRegister
    {
        public int Id { get; set; }
        public string name { get; set; }   
        public string email { get; set; }
        public long mobile { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public long pincode { get; set; }
        public string landmark { get; set; }
        public HttpPostedFileBase picture { get; set; }
    }
}