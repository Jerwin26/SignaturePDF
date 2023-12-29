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
                    Session["DocId"] =loginRepository.uploadDocuments(document);
                    document.UserId = (int)Session["UserId"];
                    document.Id= (int)Session["DocId"];

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
            Document model = TempData["MyDocument"] as Document;

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
            string filePath = Path.Combine(folderPath, "generated13.pdf");

            // Write the byte array to the file
            System.IO.File.WriteAllBytes(filePath, model.Documents);

            // Set the file path in ViewBag for later use in the view
            //ViewBag.filePath = "/SamplePDF/generated12.pdf";
            ViewBag.filePath = "/SamplePDF/generated13.pdf";
            return View();
            // Pass the model to the view
           // return View(model);
        }
        public FileResult DownloadPdf()
        {
            string filePath = Server.MapPath("~/App_Data/PdfFiles/generated.pdf");
            return File(filePath, "application/pdf", "generated.pdf");
        }
        [HttpPost]
        public ActionResult DBaction(List<SignPosition> inputValues)
        {
           
            DocSign signs = new DocSign();
            List<int> FielPages = new List<int>();
            List<int> Xaxis = new List<int>();
            List<int> Yaxis = new List<int>();
            foreach (var inputField in inputValues)
            {
                FielPages.Add(inputField.id);
                Xaxis.Add(inputField.insetTop);
                Yaxis.Add(inputField.insetLeft);
                /* var id = inputField.id;
                 var insetTop = inputField.insetTop;
                 var insetRight = inputField.insetRight;
                 var insetBottom = inputField.insetBottom;
                 var insetLeft = inputField.insetLeft;*/
            }
            /* signs.UserId = (int)Session["UserId"];
             signs.DocId= (int)Session["DocId"];*/
            signs.UserId = (int)Session["UserId"];
            signs.DocId = Session["DocId"] as int? ?? 0;
            signs.TotalFields = FielPages.Count;
            signs.FieldsPages = FielPages;
            signs.Yaxis = Yaxis;
            signs.Xaxis= Xaxis;
            LoginRepository loginRepository = new LoginRepository();
            loginRepository.AppenDocSignDetails(signs);
            // Return a response if necessary
            return Json(new { success = true, message = "Data received successfully" });
        }
        public ActionResult Details(int id)
        {
            LoginRepository loginRepository =new  LoginRepository();
            // DocSign docSign= loginRepository.GetSignValue(id);
            DocSign signs = new DocSign();
            signs.FieldsPages = new List<int> { 1 };
            signs.Xaxis = new List<int> { 651 };
            signs.Yaxis = new List<int> { 137 };
            ViewBag.filePath = "/SamplePDF/generated13.pdf";
            return View(signs);
        }

    }
}