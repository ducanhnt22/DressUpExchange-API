using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace DressUpExchange.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        public const long MAX_UPLOAD_FILE_SIZE = 25000000;//File size must lower than 25MB
        public FilesController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }
        [HttpPost]
        public async Task<ActionResult<string>> UploadFile(IFormFile file)
        {
            if (file.Length > MAX_UPLOAD_FILE_SIZE)
                return BadRequest("Exceed 25MB");
            //string url = await _fileStorageService.UploadFileToDefaultAsync(file.OpenReadStream(), file.FileName);
            string url = await _fileStorageService.UploadFileToDefaultAsyncV2(file, "picture");
            return Ok(url);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteFile(string fileName)
        {
            await _fileStorageService.DeleteFileByName(fileName);
            return Ok("Delete File sucessfully");
        }
    }
}
