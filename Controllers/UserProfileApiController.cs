using Microsoft.AspNetCore.Mvc;
using RazorTableDemo.Models;
using RazorTableDemo.Services;

namespace RazorTableDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileApiController : ControllerBase
    {
        private readonly ITaxAuthorityService _taxAuthorityService;

        public UserProfileApiController(ITaxAuthorityService taxAuthorityService)
        {
            _taxAuthorityService = taxAuthorityService;
        }

        // GET: api/UserProfileApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<S300TaxAuthority>>> GetTaxAuthorities(
            [FromQuery] string? clientCode = null,
            [FromQuery] string? authorityKey = null)
        {
            try
            {
                var results = await _taxAuthorityService.GetTaxAuthoritiesAsync(clientCode, authorityKey);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/UserProfileApi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<S300TaxAuthority>> GetTaxAuthority(int id)
        {
            try
            {
                var result = await _taxAuthorityService.GetTaxAuthorityByIdAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/UserProfileApi
        [HttpPost]
        public async Task<ActionResult<S300TaxAuthority>> CreateTaxAuthority([FromBody] S300TaxAuthority taxAuthority)
        {
            try
            {
                // Set default values
                taxAuthority.CreatedOn = DateTime.Now;
                taxAuthority.LastMaintained = DateTime.Now;
                
                var createdTaxAuthority = await _taxAuthorityService.CreateTaxAuthorityAsync(taxAuthority);
                return CreatedAtAction(nameof(GetTaxAuthority), new { id = createdTaxAuthority.Id }, createdTaxAuthority);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT: api/UserProfileApi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaxAuthority(int id, [FromBody] S300TaxAuthority taxAuthority)
        {
            if (id != taxAuthority.Id)
                return BadRequest();

            try
            {
                // Set update timestamp
                taxAuthority.UpdatedOn = DateTime.Now;
                
                var success = await _taxAuthorityService.UpdateTaxAuthorityAsync(taxAuthority);
                
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/UserProfileApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaxAuthority(int id)
        {
            try
            {
                var success = await _taxAuthorityService.DeleteTaxAuthorityAsync(id);

                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
} 