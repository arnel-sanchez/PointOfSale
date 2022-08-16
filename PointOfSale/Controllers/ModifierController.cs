using Microsoft.AspNetCore.Mvc;
using PointOfSale.DataAccess;
using PointOfSale.Models;
using PointOfSale.Models.DataBaseModels;
using PointOfSale.Models.ModifiersModels;

namespace PointOfSale.Controllers
{
    [Route("api/modifiers")]
    [ApiController]
    public class ModifierController : ControllerBase
    {
        public readonly ModifiersDataAccess _modifierService;

        public ModifierController(ModifiersDataAccess modifiersDataAccess)
        {
            _modifierService = modifiersDataAccess;
        }

        [HttpGet("get-all")]
        public IActionResult Get()
        {
            try
            {
                var modifiers = _modifierService.GetModifiers();
                return Ok(new Response<List<Modifier>> { Code = 200, Data = modifiers, Message = "Modifiers Getted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var modifiers = _modifierService.GetModifiers(id);
                return Ok(new Response<Modifier> { Code = 200, Data = modifiers, Message = "Modifier Getted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPost("add")]
        public IActionResult Post([FromBody] ModifierDTO modifier)
        {
            try
            {
                _modifierService.AddModifiers(modifier.Name, modifier.Description, modifier.Price, modifier.Add);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Modifier Added Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult Put(string id, [FromBody] ModifierDTO modifier)
        {
            try
            {
                _modifierService.UpdateModifiers(id, modifier.Name, modifier.Description, modifier.Price, modifier.Add);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Modifier Updated Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _modifierService.DeleteModifiers(id);
                return Ok(new Response<string> { Code = 200, Data = "", Message = "Modifier Deleted Success" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new Response<string> { Code = 400, Message = ex.Message, Data = "" });
            }
        }
    }
}
