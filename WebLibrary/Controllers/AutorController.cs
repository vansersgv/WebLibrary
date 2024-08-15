using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLibrary.Context;
using WebLibrary.DTOs;
using WebLibrary.Entities;
using WebLibrary.Services;

namespace WebLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController(AutorService autorService) : ControllerBase
    {
        private readonly AutorService _autorService = autorService;

        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {            
            return Ok(await _autorService.lista());
        }




    }
}
