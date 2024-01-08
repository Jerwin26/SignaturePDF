using Grpc.Core;
using HtmlAgilityPack;
using IronPdf;
using SignaturePDF.Models;
using SignaturePDF.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using Login = SignaturePDF.Models.Login;

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
           // List<Document> documents = loginRepository.GetAllDocumentsByUser((int)Session["UserId"]);
            List<Document> documents = loginRepository.GetAllDocumentsByUser(1);
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
            string filePath = Path.Combine(folderPath, "generated15.pdf");

            // Write the byte array to the file
            System.IO.File.WriteAllBytes(filePath, model.Documents);

            // Set the file path in ViewBag for later use in the view
            //ViewBag.filePath = "/SamplePDF/generated12.pdf";
            ViewBag.filePath = "/SamplePDF/generated15.pdf";
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
            List<int> Top = new List<int>();
            List<int> Right = new List<int>();
            List<int> Bottom = new List<int>();
            List<int> Left = new List<int>();
            foreach (var inputField in inputValues)
            {
                FielPages.Add(inputField.id);
                Top.Add(inputField.insetTop);
                Right.Add(inputField.insetRight);
                Bottom.Add(inputField.insetBottom);
                Left.Add(inputField.insetLeft);
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
            signs.Top = Top;
            signs.Right= Right;
            signs.Bottom = Bottom;
            signs.Left = Left;
            LoginRepository loginRepository = new LoginRepository();
            loginRepository.AppenDocSignDetails(signs);
            // Return a response if necessary
            return Json(new { success = true, message = "Data received successfully" });
        }
        [HttpPost]
        public ActionResult Position(List<SignPosition> inputValues)
        {

            DocSign signs = new DocSign();
            List<int> FielPages = new List<int>();
            List<int> Top = new List<int>();
            List<int> Right = new List<int>();
            List<int> Bottom = new List<int>();
            List<int> Left = new List<int>();
            foreach (var inputField in inputValues)
            {
                FielPages.Add(inputField.id);
                Top.Add(inputField.insetTop);
                Right.Add(inputField.insetRight);
                Bottom.Add(inputField.insetBottom);
                Left.Add(inputField.insetLeft);
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
            signs.Top = Top;
            signs.Right = Right;
            signs.Bottom = Bottom;
            signs.Left = Left;
            LoginRepository loginRepository = new LoginRepository();
           loginRepository.UpdateDocSignDetails(signs);
            // Return a response if necessary
            return Json(new { success = true, message = "Data received successfully" });
        }

        [HttpPost]
        public ActionResult SignData(List<SignPosition> inputValues)
        {

            LoginRepository loginRepository = new LoginRepository();
            foreach (var inputField in inputValues)
            {
                inputField.Bytes= Convert.FromBase64String(inputField.base64Data);
                inputField.DocId = Session["DocId"] as int? ?? 0;
                loginRepository.saveDatew(inputField);
            }
            /* signs.UserId = (int)Session["UserId"];
             signs.DocId= (int)Session["DocId"];*//*
            signs.UserId = (int)Session["UserId"];
            signs.DocId = Session["DocId"] as int? ?? 0;
            signs.TotalFields = FielPages.Count;
            signs.FieldsPages = FielPages;
            signs.Top = Top;
            signs.Right = Right;
            signs.Bottom = Bottom;
            signs.Left = Left;
            LoginRepository loginRepository = new LoginRepository();*/
            //loginRepository.AppenDocSignDetails(signs);
            // Return a response if necessary
            return Json(new { success = true, message = "Data received successfully" });
        }
        public ActionResult Details(int id)
        {
            LoginRepository loginRepository = new LoginRepository();
            DocSign signs = loginRepository.GetSignValue(id);
            Session["DocId"] = id;
            ViewBag.filePath = "/SamplePDF/generated15.pdf";
            return View(signs);
        }
        public ActionResult Preview(int id)
        {
            LoginRepository loginRepository = new LoginRepository();
            DocSign signs = loginRepository.GetSignValue(id);
            Session["DocId"] = id;
            ViewBag.filePath = "/SamplePDF/generated15.pdf";
            return View(signs);
        }
        
        /*public ActionResult Details()
        {
            LoginRepository loginRepository = new LoginRepository();
            DocSign signs = loginRepository.GetSignValue(1);

            ViewBag.filePath = "/SamplePDF/generated14.pdf";
            return View(signs);
        }*/
        [HttpGet]
        public ActionResult Index1(string content)
        {
            ChromePdfRenderer renderer = new ChromePdfRenderer();
            /*renderer.RenderingOptions.FirstPageNumber = 1;
            // Header options
            renderer.RenderingOptions.TextHeader.DrawDividerLine = true;
            renderer.RenderingOptions.TextHeader.CenterText = "{url}";
            renderer.RenderingOptions.TextHeader.Font = IronSoftware.Drawing.FontTypes.Helvetica;
            renderer.RenderingOptions.TextHeader.FontSize = 12;
            // Footer options
            renderer.RenderingOptions.TextFooter.DrawDividerLine = true;
            renderer.RenderingOptions.TextHeader.Font = IronSoftware.Drawing.FontTypes.Arial;
            renderer.RenderingOptions.TextFooter.FontSize = 10;
            renderer.RenderingOptions.TextFooter.LeftText = "{date} {time}";
            renderer.RenderingOptions.TextFooter.RightText = "{page} of {total-pages}";*/
            PdfDocument pdf = renderer.RenderHtmlAsPdf(content);
            string filePath = Server.MapPath("~/App_Data/html-string.pdf");
            pdf.SaveAs(filePath);
            var PDF = IronPdf.ChromePdfRenderer.StaticRenderUrlAsPdf(new Uri("https://localhost:44348/Login/Details"));
            return File(PDF.BinaryData, "application/pdf", "Wiki.Pdf");
        }
        [HttpPost]
        public ActionResult ProcessHtml()
        {
            string outerHtml = TempData["OuterHtml"] as string;

            // Your logic to process the outerHtml here

            // For example, you can return a JSON result
            return Json(new { success = true, message = "Html processed successfully" });
        }

        [HttpPost]
        public ActionResult SetTempData(Elements outerHtml)
        {
            try
            {
                TempData["OuterHtml"] = outerHtml;
                return Json(new { success = true, message = "TempData set successfully" });
            }
            catch (Exception ex)
            {
                /*ChromePdfRenderer renderer = new ChromePdfRenderer();
     /*renderer.RenderingOptions.FirstPageNumber = 1;
     // Header options
     renderer.RenderingOptions.TextHeader.DrawDividerLine = true;
     renderer.RenderingOptions.TextHeader.CenterText = "{url}";
     renderer.RenderingOptions.TextHeader.Font = IronSoftware.Drawing.FontTypes.Helvetica;
     renderer.RenderingOptions.TextHeader.FontSize = 12;
     // Footer options
     renderer.RenderingOptions.TextFooter.DrawDividerLine = true;
     renderer.RenderingOptions.TextHeader.Font = IronSoftware.Drawing.FontTypes.Arial;
     renderer.RenderingOptions.TextFooter.FontSize = 10;
     renderer.RenderingOptions.TextFooter.LeftText = "{date} {time}";
     renderer.RenderingOptions.TextFooter.RightText = "{page} of {total-pages}";*/
               /* PdfDocument pdf = renderer.RenderHtmlAsPdf(content);
                string filePath = Server.MapPath("~/App_Data/html-string.pdf");
                pdf.SaveAs(filePath);
                var PDF = IronPdf.ChromePdfRenderer.StaticRenderUrlAsPdf(new Uri("https://localhost:44348/Login/Details"));
                //return File(PDF.BinaryData, "application/pdf", "Wiki.Pdf");
                */
                // Log the exception for further analysis*/
                // You can use a logging framework like Serilog or log to a file
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "An error occurred while setting TempData" });
            }
        }
        [HttpPost]
        public ActionResult SetAndProcessHtml(Elements elements)
        {
            try
            {
                string outerHtml = elements.devEle;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(elements.devEle);
                HtmlNode viewerElement = doc.DocumentNode.SelectSingleNode("//div[@id='viewer']");
                string htmlStringWithoutBackslashes = outerHtml.Replace("\\", "");
                Console.WriteLine(viewerElement.OuterHtml);
                ChromePdfRenderer renderer = new ChromePdfRenderer();
                PdfDocument pdf = renderer.RenderHtmlAsPdf(htmlStringWithoutBackslashes);
                string filePath = Server.MapPath("~/App_Data/html-string.pdf");
                pdf.SaveAs(filePath);
                var responseData = new
                {
                    success = true,
                    message = "Html processed successfully",
                    additionalData = "Your additional data here"
                };

                return Json(responseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                var errorResponse = new
                {
                    success = false,
                    message = "An error occurred while processing Html"
                };

                return Json(errorResponse);
            }
        }
        public FileResult DownloadPdf1()
        {
            string filePath = Server.MapPath("~/SamplePDF/generated15.pdf");
            return File(filePath, "application/pdf", "generated15.pdf");
        }
        [HttpGet]
        public FileResult DownloadPdfWala(string itemId)
        {
            
            string fullUrl = Request.Url.ToString();
            Console.WriteLine(fullUrl + " moni");
            string filePath = Server.MapPath("~/SamplePDF/generated15.pdf");
            return File(filePath, "application/pdf", "generated15.pdf");
        }
        public ActionResult Preview1()
        {

            return View();
        }
        [HttpGet]
        public JsonResult AjaxCall(int docId)
        {

            LoginRepository loginRepository =new LoginRepository();
            //Console.WriteLine(docId.docId);
            var data = loginRepository.GetAllDataByDocId(docId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}