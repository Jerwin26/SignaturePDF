using SignaturePDF.Models;
using SignaturePDF.Repository;
using System;
using System.Collections.Generic;
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
                List<Document> documents = loginRepository.GetAllDocumentsByUser(registration.Id);
                if(documents != null)
                {
                    return View("UserDoc",documents);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult UserDoc(List<Document> documents)
        {
            
            return View(documents);
        }

    }
}