using Microsoft.AspNetCore.Http;

namespace UploadDocuments.Models
{
    public class FileUploadModel
    {
        public IFormFile File { get; set; }
    }
}
