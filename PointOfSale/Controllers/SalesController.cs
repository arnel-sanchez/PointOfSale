using Microsoft.AspNetCore.Mvc;
using PointOfSale.DataAccess;
using PointOfSale.Models;
using PointOfSale.Models.DataBaseModels;
using PointOfSale.Models.SalesModels;

namespace PointOfSale.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesDataAccess _salesDataAccess;

        public SalesController(ISalesDataAccess salesDataAccess)
        {
            _salesDataAccess = salesDataAccess;
        }

        [HttpGet("get")]
        public IActionResult Get([FromBody] GetSalesDTO data)
        {
            try
            {
                var item = _salesDataAccess.GetSales(data.DateTime, data.SellerId);
                return Ok(new Response<List<Sale>> { Code = 200, Data = item, Message = "Sale Getted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPost("add")]
        public IActionResult Post([FromBody] SaleDTO sale)
        {
            try
            {
                List<(string, List<string>)> items = new List<(string, List<string>)>();
                foreach (var item in sale.Items)
                {
                    items.Add((item.ItemId, item.ModifiersId));
                }
                _salesDataAccess.AddSale(sale.SellerID, items);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Sale Added Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult Put(string id, [FromBody] SaleDTO sale)
        {
            try
            {
                List<(string, List<string>)> items = new List<(string, List<string>)>();
                foreach (var item in sale.Items)
                {
                    items.Add((item.ItemId, item.ModifiersId));
                }
                _salesDataAccess.AddSale(sale.SellerID, items);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Sale Updated Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpDelete("delete/{id}/{userId}")]
        public IActionResult Delete(string id, string userId)
        {
            try
            {
                _salesDataAccess.DeleteSale(userId, id);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Sale Deleted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpDelete("delete-all/{userId}")]
        public IActionResult Delete(string userId)
        {
            try
            {
                _salesDataAccess.DeleteSales(userId);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Sales Deleted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }
    }
}
