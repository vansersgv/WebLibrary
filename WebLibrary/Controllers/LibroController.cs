using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebLibrary.DTOs;
using WebLibrary.Services;

namespace WebLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly LibroService _libroService;

        public LibroController(LibroService libroService)
        {
            _libroService = libroService;
        }

        [HttpGet]
        [Route("lista")]
        [SwaggerOperation(Summary = "Obtiene la lista de todos los libros")]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            var listaDTO = await _libroService.GetLibrosAsync();
            return Ok(listaDTO);
        }

        [HttpGet]
        [Route("buscar/{nombre}")]
        [SwaggerOperation(Summary = "Busca un libro por su nombre")]
        public async Task<ActionResult<LibroDTO>> Get(string nombre)
        {
            var libroDTO = await _libroService.GetLibroByNombreAsync(nombre);
            if (libroDTO == null)
            {
                return NotFound("Libro no encontrado");
            }
            return Ok(libroDTO);
        }

        [HttpPost]
        [Route("crear")]
        [SwaggerOperation(Summary = "Crea un nuevo libro")]
        public async Task<ActionResult> Crear([FromBody] LibroDTO libroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _libroService.CrearLibroAsync(libroDTO);
            return Ok("Libro agregado");
        }

        [HttpPut]
        [Route("editar")]
        [SwaggerOperation(Summary = "Edita un libro existente")]
        public async Task<ActionResult> Editar([FromBody] LibroDTO libroDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _libroService.EditarLibroAsync(libroDTO);
            if (!result)
            {
                return NotFound("Libro no encontrado");
            }
            return Ok("Libro editado");
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        [SwaggerOperation(Summary = "Elimina un libro por su ID")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var result = await _libroService.EliminarLibroAsync(id);
            if (!result)
            {
                return NotFound("Libro no encontrado");
            }
            return Ok("Libro eliminado");
        }
    }
}
