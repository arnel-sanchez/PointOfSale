using Microsoft.AspNetCore.Mvc;
using PointOfSale.DataAccess;
using PointOfSale.Models;
using PointOfSale.Models.DataBaseModels;
using PointOfSale.Models.ItemsModels;
using PointOfSale.Services;

namespace PointOfSale.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsDataAccess _itemDataAccess;
        private readonly IFileHandler _fileHandler;

        public ItemsController(IItemsDataAccess itemDataAccess, IFileHandler fileHandler)
        {
            _itemDataAccess = itemDataAccess;
            _fileHandler = fileHandler;
        }

        [HttpGet("get-all")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            try
            {
                var items = _itemDataAccess.GetItems();
                return Ok(items);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id}")]
        [Produces("application/json")]
        public IActionResult Get(string id)
        {
            try
            {
                var item = _itemDataAccess.GetItems(id);
                return Ok(item);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        [Produces("application/json")]
        public IActionResult Post([FromBody] ItemsDTO item)
        {
            try
            {
                var qr = QREncoderService.GenerateQRCode(item.Code);
                var code = _fileHandler.GetURL() + _fileHandler.UploadFile(qr);
                var image = _fileHandler.GetURL() + item.Image;
                if (item.Image.Equals("images/item.webp"))
                {
                    image = item.Image;
                }
                _itemDataAccess.AddItem(item.Name, item.Price, item.Description, item.Quantity, item.Category, image, item.Code,code, item.ModifiersId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [Produces("application/json")]
        public IActionResult Put(string id, [FromBody] ItemsDTO item)
        {
            try
            {
                var itemTemp = _itemDataAccess.GetItems(id);
                _fileHandler.DeleteFile(itemTemp.Code);
                if (!itemTemp.Image.Equals("images/item.webp"))
                {
                    _fileHandler.DeleteFile(itemTemp.Image);
                }
                var qr = QREncoderService.GenerateQRCode(item.Code);
                var code = _fileHandler.GetURL() + _fileHandler.UploadFile(qr);
                var image = _fileHandler.GetURL() + item.Image;
                if (item.Image.Equals("images/item.webp"))
                {
                    image = item.Image;
                }
                _itemDataAccess.UpdateItem(id, item.Name, item.Price, item.Description, item.Quantity, item.Category, image, item.Code, code, item.ModifiersId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("assign/{itemId}/{modifierId}")]
        [Produces("application/json")]
        public IActionResult Put(string itemId, string modifierId)
        {
            try
            {
                _itemDataAccess.AssignDisassign(itemId, modifierId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Produces("application/json")]
        public IActionResult Delete(string id)
        {
            try
            {
                var item = _itemDataAccess.GetItems(id);
                _fileHandler.DeleteFile(item.Code);
                if (!item.Image.Equals("images/item.webp"))
                {
                    _fileHandler.DeleteFile(item.Image);
                }
                _itemDataAccess.DeleteItem(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
