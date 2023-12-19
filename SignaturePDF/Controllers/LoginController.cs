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

                return DisplayPdf1(document);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DisplayPdf1(Document document)
        {
            if (document.Documents != null && document.Documents.Length > 0)
            {
                ViewBag.fileBytes = Convert.ToBase64String(document.Documents);
            }
            else
            {
              
                ViewBag.fileBytes = null; 
            }
            return View(document);
        }

    }
}