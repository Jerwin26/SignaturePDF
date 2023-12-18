using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignaturePDF.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public Status Status { get; set; }
        public byte[] Documents { get; set; }


    }
}