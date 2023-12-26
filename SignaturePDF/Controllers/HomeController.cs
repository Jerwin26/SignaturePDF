using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignaturePDF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult HelloPdf()
        {
            return View();
        }public ActionResult Manage()
        {
            return View();
        }

        public ActionResult DisplayPdf()
        {
            // Specify the path to your PDF file
            string pdfFilePath = Server.MapPath("~/App_Data/PdfFiles/GenerateDocument.pdf");

            // Set the response content type to PDF
            Response.ContentType = "application/pdf";

            // Optionally, set a content disposition header to force the browser to download the file
            Response.AppendHeader("Content-Disposition", "inline; filename=GenerateDocument.pdf");

            // Write the PDF file content directly to the response stream
            Response.WriteFile(pdfFilePath);

            return null; // Return null to avoid rendering a view
        }

    }
}