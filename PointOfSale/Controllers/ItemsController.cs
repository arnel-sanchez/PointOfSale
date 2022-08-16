using Microsoft.AspNetCore.Mvc;
using PointOfSale.DataAccess;
using PointOfSale.Models;
using PointOfSale.Models.DataBaseModels;
using PointOfSale.Models.ItemsModels;

namespace PointOfSale.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsDataAccess _itemDataAccess;

        public ItemsController(IItemsDataAccess itemDataAccess)
        {
            _itemDataAccess = itemDataAccess;
        }

        [HttpGet("get-all")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            try
            {
                var items = _itemDataAccess.GetItems();
                return Ok(new Response<List<Item>> { Code = 200, Data = items, Message = "Items Getted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpGet("get/{id}")]
        [Produces("application/json")]
        public IActionResult Get(string id)
        {
            try
            {
                var item = _itemDataAccess.GetItems(id);
                return Ok(new Response<Item> { Code = 200, Data = item, Message = "Item Getted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPost("add")]
        [Produces("application/json")]
        public IActionResult Post([FromBody] ItemsDTO item)
        {
            try
            {
                _itemDataAccess.AddItem(item.Name, item.Price, item.Description, item.Quantity, item.Category, item.Image, item.Code, item.ModifiersId);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Item Added Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPut("update/{id}")]
        [Produces("application/json")]
        public IActionResult Put(string id, [FromBody] ItemsDTO item)
        {
            try
            {
                _itemDataAccess.UpdateItem(id, item.Name, item.Price, item.Description, item.Quantity, item.Category, item.Image, item.Code, item.ModifiersId);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Item Updated Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpDelete("delete/{id}")]
        [Produces("application/json")]
        public IActionResult Delete(string id)
        {
            try
            {
                _itemDataAccess.DeleteItem(id);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Item Deleted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }
    }
}
