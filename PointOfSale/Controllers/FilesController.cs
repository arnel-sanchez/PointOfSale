using Microsoft.AspNetCore.Cors;
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
                return Ok(file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileUrl = _fileHandler.UploadFile(file);
                return Ok(fileUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody] string url)
        {
            try
            {
                _fileHandler.DeleteFile(url);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
