using System.IO;
using System.Linq;
using System.Web.Mvc;
using Ictshop.Models;

namespace Ictshop.Areas.Admin.Controllers
{
    public class FileController : Controller
    {
        private QLdienthoaiEntities db = new QLdienthoaiEntities();

        // GET: Admin/File
        public ActionResult Index()
        {
            return View();
        }
        public FileResult Download(string fileName)
        {
            var FileVirtualPath = "~/DocumentFiles/" + fileName;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }
       public FileResult DownloadProductDetails(string fileName)
{
    // Fetch data from the database
    var productDetails = db.Sanphams.ToList()
        .Select(p => $"{p.Masp} - {p.Tensp} - {p.Giatien} - {p.Soluong} -  {p.Thesim} - {p.Hangsanxuat.Tenhang} - {p.Hedieuhanh.Tenhdh}");

    var content = string.Join("\n", productDetails);

    // Set the file path to a temporary directory
    var tempDirectory = Server.MapPath("~/App_Data/TempDownloads");
    if (!Directory.Exists(tempDirectory))
    {
        Directory.CreateDirectory(tempDirectory);
    }

    var filePath = Path.Combine(tempDirectory, "ProductDetail.txt");
    System.IO.File.WriteAllText(filePath, content);

    // Provide the correct virtual path for the download
    var fileVirtualPath = "~/App_Data/TempDownloads/ProductDetail.txt";

    return File(fileVirtualPath, "text/plain", Path.GetFileName(fileVirtualPath));
}

    }
}
