using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoHosting.Services.DTO;
using Microsoft.AspNetCore.Authorization;

namespace MyWebProject.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api")]
    [ApiController]
    public class ComentaryController : ControllerBase
    {
        private IComentaryService ComentaryService;
        public ComentaryController(IComentaryService service)
        {
            ComentaryService = service;
        }

        [HttpGet]
        [Route("comentary/{id}")]
        public async Task<ActionResult> GetComentariesByVideoId(int id)
        {
            IEnumerable<ComentaryDTO> comentaryDTO = await ComentaryService.GetComentariesByVideoId(id);
            return Ok(comentaryDTO);
        }

        [HttpPost]
        [Route("comentary")]
        public async Task<ActionResult> CreateComentary(ComentaryDTO model)
        {
            if (ModelState.IsValid)
            {
                await ComentaryService.AddComentary(model);
                return Ok("You added comentary");
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("comentaries/{id}")]
        public async Task<ActionResult> DeleteComentary(int id)
        {
            ComentaryDTO comentary = await ComentaryService.GetComentaryById(id);
            if (comentary.UserId == User.Identity.Name || User.IsInRole("Admin"))
            {
                await ComentaryService.RemoveComentary(id);
                return Ok("This comentary was deleted");
            }
            return Unauthorized();

        }
    }
}
