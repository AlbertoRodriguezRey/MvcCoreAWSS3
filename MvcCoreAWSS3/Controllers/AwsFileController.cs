using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSS3.Services;

namespace MvcCoreAWSS3.Controllers
{
    public class AwsFileController : Controller
    {
        private ServiceStorageS3 service;

        public AwsFileController(ServiceStorageS3 service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            List<string> files = this.service.GetFilesAsync().Result;
            ViewData["MENSAJE"] = TempData["MENSAJE"];
            return View(files);
        }

        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            TempData["MENSAJE"] = $"Fichero {fileName} eliminado correctamente";
            return RedirectToAction("Index");
        }

        public IActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            int codigo = 0;
            using (Stream stream = file.OpenReadStream())
            {
                codigo = await this.service.UploadFileAsync(file.FileName, stream);
            }
            TempData["MENSAJE"] = $"Http Status Code: {codigo}";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PrivateFile(string fileName)
        {
            Stream stream = await this.service.GetPrivateFileAsync(fileName);
            return File(stream, "image/png");
        }
    }
}
