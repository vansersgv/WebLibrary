using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLibrary.Context;
using WebLibrary.DTOs;
using WebLibrary.Entities;


namespace WebLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly AppDbContext _context; 
        public LibroController(AppDbContext context)
        {
            _context=context;
        }
        
        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            var listaDTO = new List<LibroDTO>();
            var listaDB = await _context.Libros.Include(a => a.AutorReferencia). ToListAsync();
            foreach (var item in listaDB)
            {
                listaDTO.Add(new LibroDTO
                {
                    IdLibro = item.IdLibro,
                    Titulo = item.Titulo,
                    Descripcion = item.Descripcion,
                    FechaDePublicacion = item.FechaDePublicacion,
                    AutorId = item.AutorId
                });
            }
            return Ok(listaDTO);
        }

        [HttpGet]
        [Route("buscar/{id}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        { 
            var libroDTO = new LibroDTO();
            var libroDB = await _context.Libros.Include(a => a.AutorReferencia)
                .Where(x => x.IdLibro == id).FirstAsync();

            libroDTO.IdLibro = id;
            libroDTO.Titulo = libroDB.Titulo;
            libroDTO.Descripcion = libroDB.Descripcion;
            libroDTO.FechaDePublicacion = libroDB.FechaDePublicacion;
            libroDTO.AutorId = libroDB.AutorId;
            return Ok(libroDTO);
        }                  



            [HttpPost]
        [Route("crear")]
        public async Task<ActionResult> crear(LibroDTO libroDTO)
        {
            var libroDB = new Libro
            {
                Titulo = libroDTO.Titulo,
                Descripcion = libroDTO.Descripcion,
                FechaDePublicacion = libroDTO.FechaDePublicacion,
                AutorId = libroDTO.AutorId
            };
            _context.Libros.Add(libroDB);
            await _context.SaveChangesAsync();
            return Ok("Libro agregado");
        }

        [HttpPut]
        [Route("editar")]
        public async Task<ActionResult> Editar(LibroDTO libroDTO)
        {
            var libroDB = await _context.Libros.Include(a => a.AutorReferencia)
                .Where(x => x.IdLibro == libroDTO.IdLibro).FirstAsync();
            libroDB.Titulo = libroDTO.Titulo;
            libroDB.Descripcion = libroDTO.Descripcion;
            libroDB.FechaDePublicacion = libroDTO.FechaDePublicacion;
            libroDB.AutorId = libroDTO.AutorId;
            _context.Libros.Update(libroDB);
            await _context.SaveChangesAsync();
            return Ok("Libro editado");
        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var libroDB = await _context.Libros.FindAsync(id);
            if (libroDB == null)
            {
                return NotFound("Libro no encontrado");
            }
            _context.Libros.Remove(libroDB);
            await _context.SaveChangesAsync();
            return Ok("Libro eliminado");
        }
    }
}
