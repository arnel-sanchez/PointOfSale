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
                var sales = _salesDataAccess.GetSales(data.DateTime, data.SellerId);
                return Ok(sales);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
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
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
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
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}/{userId}")]
        public IActionResult Delete(string id, string userId)
        {
            try
            {
                _salesDataAccess.DeleteSale(userId, id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-all/{userId}")]
        public IActionResult Delete(string userId)
        {
            try
            {
                _salesDataAccess.DeleteSales(userId);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
