using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignaturePDF.Models
{
    public class DocSign
    {
        public int Id { get; set; }
        public int DocId { get; set; }

        public int UserId { get; set; }
        public int TotalFields { get; set; }
        public List<int> FieldsPages { get; set; }
        public List<int> Top {get;set;}
        public List<int> Right { get; set; }
        public List<int> Bottom { get; set; }
        public List<int> Left { get; set; }

    }
}