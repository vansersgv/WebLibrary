using Microsoft.EntityFrameworkCore;
using WebLibrary.Context;
using WebLibrary.DTOs;
using WebLibrary.Entities;

namespace WebLibrary.Services
{
    public class LibroService
    {
        private readonly AppDbContext _context;

        public LibroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LibroDTO>> GetLibrosAsync()
        {
            var listaDTO = new List<LibroDTO>();
            var listaDB = await _context.Libros.Include(a => a.Autor).ToListAsync();
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
            return listaDTO;
        }

        public async Task<LibroDTO> GetLibroByNombreAsync(string nombre)
        {
            var libroDB = await _context.Libros.Include(a => a.Autor)
                .Where(x => x.Titulo == nombre).FirstOrDefaultAsync();

            if (libroDB == null)
            {
                return null;
            }

            return new LibroDTO
            {
                IdLibro = libroDB.IdLibro,
                Titulo = libroDB.Titulo,
                Descripcion = libroDB.Descripcion,
                FechaDePublicacion = libroDB.FechaDePublicacion,
                AutorId = libroDB.AutorId
            };
        }

        public async Task CrearLibroAsync(LibroDTO libroDTO)
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
        }

        public async Task<bool> EditarLibroAsync(LibroDTO libroDTO)
        {
            var libroDB = await _context.Libros.Include(a => a.Autor)
                .Where(x => x.IdLibro == libroDTO.IdLibro).FirstOrDefaultAsync();

            if (libroDB == null)
            {
                return false;
            }

            libroDB.Titulo = libroDTO.Titulo;
            libroDB.Descripcion = libroDTO.Descripcion;
            libroDB.FechaDePublicacion = libroDTO.FechaDePublicacion;
            libroDB.AutorId = libroDTO.AutorId;

            _context.Libros.Update(libroDB);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarLibroAsync(int id)
        {
            var libroDB = await _context.Libros.FindAsync(id);
            if (libroDB == null)
            {
                return false;
            }
            _context.Libros.Remove(libroDB);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
