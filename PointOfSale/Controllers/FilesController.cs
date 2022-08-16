using Microsoft.AspNetCore.Mvc;
using PointOfSale.Models;
using PointOfSale.Services;

namespace PointOfSale.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileHandler _fileHandler;

        public FilesController(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        [HttpPost("download")]
        public IActionResult Download([FromBody] string url)
        {
            try
            {
                var file = _fileHandler.DownloadFile(url).Result;
                return Ok(new Response<MemoryStream> { Data = file, Code = 200, Message = "File Downloaded Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            try
            {
                var fileUrl = _fileHandler.UploadFile(file);
                return Ok(new Response<string> { Data = fileUrl, Code = 200, Message = "File Uploaded Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody] string url)
        {
            try
            {
                _fileHandler.DeleteFile(url);
                return Ok(new Response<string> { Data = "", Code = 200, Message = "File Deleted Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }
    }
}
