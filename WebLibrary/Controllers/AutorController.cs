using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebLibrary.DTOs;
using WebLibrary.Services;

namespace WebLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly AutorService _autorService;

        public AutorController(AutorService autorService)
        {
            _autorService = autorService;
        }

        [HttpGet]
        [Route("lista")]
        [SwaggerOperation(Summary = "Obtiene la lista de todos los autores")]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            return Ok(await _autorService.Lista());
        }

        [HttpGet]
        [Route("buscar/{nombre}")]
        [SwaggerOperation(Summary = "Busca un autor por su nombre")]
        public async Task<ActionResult<AutorDTO>> Get(string nombre)
        {
            var autorDTO = await _autorService.GetAutorByNombreAsync(nombre);
            if (autorDTO == null)
            {
                return NotFound("Autor no encontrado");
            }
            return Ok(autorDTO);
        }

        [HttpPost]
        [Route("crear")]
        [SwaggerOperation(Summary = "Crea un nuevo autor")]
        public async Task<ActionResult> Crear(AutorDTO autorDTO)
        {
            await _autorService.CrearAutorAsync(autorDTO);
            return Ok("Autor agregado");
        }

        [HttpPut]
        [Route("editar")]
        [SwaggerOperation(Summary = "Edita un autor existente")]
        public async Task<ActionResult> Editar(AutorDTO autorDTO)
        {
            var result = await _autorService.EditarAutorAsync(autorDTO);
            if (!result)
            {
                return NotFound("Autor no encontrado");
            }
            return Ok("Autor editado");
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        [SwaggerOperation(Summary = "Elimina un autor por su ID")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var result = await _autorService.EliminarAutorAsync(id);
            if (!result)
            {
                return NotFound("Autor no encontrado");
            }
            return Ok("Autor eliminado");
        }

        [HttpGet]
        [Route("libros/{nombre}")]
        [SwaggerOperation(Summary = "Obtiene los libros de un autor por su nombre")]
        public async Task<ActionResult<List<LibroDTO>>> GetLibrosByAutorNombre(string nombre)
        {
            var librosDTO = await _autorService.GetLibrosByAutorNombreAsync(nombre);
            if (librosDTO == null)
            {
                return NotFound("Autor no encontrado o no tiene libros");
            }
            return Ok(librosDTO);
        }
    }
}
