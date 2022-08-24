using Microsoft.AspNetCore.Mvc;
using PointOfSale.DataAccess;
using PointOfSale.Models;
using PointOfSale.Models.DataBaseModels;
using PointOfSale.Models.ItemsModels;
using PointOfSale.Models.ModifiersModels;

namespace PointOfSale.Controllers
{
    [Route("api/modifiers")]
    [ApiController]
    public class ModifierController : ControllerBase
    {
        public readonly IModifiersDataAccess _modifierService;

        public ModifierController(IModifiersDataAccess modifiersDataAccess)
        {
            _modifierService = modifiersDataAccess;
        }

        [HttpGet("get-all")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            try
            {
                var modifiers = _modifierService.GetModifiers();
                return Ok(modifiers);
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
                var modifier = _modifierService.GetModifiers(id);
                return Ok(modifier);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        [Produces("application/json")]
        public IActionResult Post([FromBody] ModifierDTO modifier)
        {
            try
            {
                _modifierService.AddModifiers(modifier.Name, modifier.Description, modifier.Price, modifier.Add);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        [Produces("application/json")]
        public IActionResult Put(string id, [FromBody] ModifierDTO modifier)
        {
            try
            {
                _modifierService.UpdateModifiers(id, modifier.Name, modifier.Description, modifier.Price, modifier.Add);
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
                _modifierService.DeleteModifiers(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
