using SignaturePDF.Models;
using SignaturePDF.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignaturePDF.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Login login = new Login();
            return View(login);
        }
        [HttpPost]
        public ActionResult Index(Login login)
        {
           LoginRepository loginRepository = new LoginRepository();
           Registration registration= loginRepository.GetUser(login);   
            if(registration.UserName != null)
            {
                Session["UserId"] = registration.Id;
                return RedirectToAction("UserDoc");
            }
            return View();
        }

        [HttpGet]
        public ActionResult UserDoc()
        {
            LoginRepository loginRepository = new LoginRepository();
            List<Document> documents = loginRepository.GetAllDocumentsByUser((int)Session["UserId"]);
            return View(documents);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Document document = new Document(); 
            document.UserId= (int)Session["UserId"];
            return View(document);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    byte[] fileBytes;
                    using (BinaryReader reader = new BinaryReader(file.InputStream))
                    {
                        fileBytes = reader.ReadBytes(file.ContentLength);
                    }

                    document.Documents = fileBytes;
                    LoginRepository loginRepository = new LoginRepository();
                    document.UserId = (int)Session["UserId"];

                }
                TempData["MyDocument"] = document;
                return RedirectToAction("DisplayPdf1");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DisplayPdf1()
        {
            //Document model = null;
            // Retrieve the document model from TempData
             /*Document model = TempData["MyDocument"] as Document;

             // Ensure the model is not null
             if (model == null)
             {
                 // Handle the case where the model is null, for example, redirect to another action
                 return RedirectToAction("Index");
             }

             // Set the folder path where the PDF file will be saved
             string folderPath = Server.MapPath("~/SamplePDF");

             // Ensure the folder exists; create it if necessary
             if (!Directory.Exists(folderPath))
             {
                 Directory.CreateDirectory(folderPath);
             }

             // Combine the folder path with the desired file name
             string filePath = Path.Combine(folderPath, "generated12.pdf");

             // Write the byte array to the file
             System.IO.File.WriteAllBytes(filePath, model.Documents);*/

             // Set the file path in ViewBag for later use in the view
            //ViewBag.filePath = "/SamplePDF/generated12.pdf";
            ViewBag.filePath = "/SamplePDF/generated12.pdf";
            return View();
            // Pass the model to the view
           // return View(model);
        }
        public FileResult DownloadPdf()
        {
            string filePath = Server.MapPath("~/App_Data/PdfFiles/generated.pdf");
            return File(filePath, "application/pdf", "generated.pdf");
        }



    }
}