using DropZoneUploadMVC5.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DropZoneUploadMVC5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveUploadFile(int CategoryId)
        {
            var fName = string.Empty;
            Photo result = new Photo();

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                fName = file.FileName;
                if(file != null && file.ContentLength > 0)
                {
                    string UploadFolder = "Category_" + CategoryId;
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));
                    string pathString = Path.Combine(originalDirectory.ToString(), UploadFolder);

                    bool isExists = Directory.Exists(pathString);
                    if (!isExists)
                        Directory.CreateDirectory(pathString);

                    string RandomName = Guid.NewGuid() + Path.GetExtension(fName);
                    var path = string.Format("{0}\\{1}", pathString, RandomName);
                    file.SaveAs(path);

                    result.Name = fName;
                    result.Url = "/Uploads/" + UploadFolder + "/" + RandomName;
                }
            }

            return Json(new { Name = result.Name, UrlPhoto = result.Url }, JsonRequestBehavior.AllowGet);
        }

    }
}