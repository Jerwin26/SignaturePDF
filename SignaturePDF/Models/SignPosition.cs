using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignaturePDF.Models
{
    public class SignPosition
    {
        public int id { get; set; }
        public int insetTop { get; set; }
        public int insetRight { get; set; }
        public int insetBottom { get; set; }
        public int insetLeft { get; set; }
        public string base64Data { get; set; }

        public int DocId { get; set; }

        public byte[] Bytes { get; set; }

        public int SignId { get; set; }

    }
}