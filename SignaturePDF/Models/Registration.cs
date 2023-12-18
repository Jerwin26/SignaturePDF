using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignaturePDF.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}