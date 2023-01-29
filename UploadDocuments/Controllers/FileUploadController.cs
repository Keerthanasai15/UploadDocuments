using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UploadDocuments.Models;

namespace UploadDocuments.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IConfiguration _configuration;
       

        public FileUploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>UploadFile(FileUploadModel model)
        {
            if (model.File == null || model.File.Length==0)
            {
                return Content("file not Selected");
            
            }
            var client = new HttpClient();
            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(model.File.OpenReadStream()), "file", model.File.FileName);
            var response = await client.PostAsync("ApiUrl:api", form);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Upload Failed");
            }
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDocument(int id)
        {
            FileUploadModel file = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"FileUpload/GetFileById");
                //HttpResponseMessage response = await client.GetAsync($"FileUpload/GetFileById");
                // byte[] content = await response.Content.ReadAsByteArrayAsync();

                if (result.IsSuccessStatusCode)
                {
                    // using (var stream=await response.Content.ReadAsStreamAsync())
                    // {
                    //  var bytes=new byte[stream.Length];
                    //  await stream.ReadAsync(bytes,0, (int)stream.Length);
                    // }
                    file = await result.Content.ReadAsAsync<FileUploadModel>();
                    // var document = this.GetDocument(id);
                    // return File(document.Content, document.ContentType, document.FileName);

                }
            }
            
           return View(file);
             
        }



        
        
    }
}
