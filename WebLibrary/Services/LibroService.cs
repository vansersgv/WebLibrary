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
                    AutorId = item.AutorId,
                    AutorNombre = item.Autor.Nombre, 
                    AutorNacionalidad = item.Autor.Nacionalidad,
                    AutorFechaNacimiento = item.Autor.FechaNacimiento,
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
                AutorId = libroDB.AutorId,
                AutorNombre = libroDB.Autor.Nombre,                
            };
        }

        public async Task CrearLibroAsync(LibroDTO libroDTO)
        {
            // Buscar el autor por nombre
            var autor = await _context.Autores.FirstOrDefaultAsync(a => a.Nombre == libroDTO.AutorNombre);

            if (autor == null)
            {
                // Crear un nuevo autor si no existe
                autor = new Autor
                {
                    Nombre = libroDTO.AutorNombre,
                    Nacionalidad = libroDTO.AutorNacionalidad ?? "Desconocido", // Asignar "Desconocido" si está vacío
                    FechaNacimiento = libroDTO.AutorFechaNacimiento // Manejar si es nulo en la clase Autor
                };
                _context.Autores.Add(autor);
                await _context.SaveChangesAsync();
            }

            var libroDB = new Libro
            {
                Titulo = libroDTO.Titulo,
                Descripcion = libroDTO.Descripcion,
                FechaDePublicacion = libroDTO.FechaDePublicacion,
                AutorId = autor.IdAutor // Usar el Id del autor existente o recién creado
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

            // Buscar el autor por nombre
            var autor = await _context.Autores.FirstOrDefaultAsync(a => a.Nombre == libroDTO.AutorNombre);

            if (autor == null)
            {
                // Crear un nuevo autor si no existe
                autor = new Autor
                {
                    Nombre = libroDTO.AutorNombre,
                    Nacionalidad = libroDTO.AutorNacionalidad ?? "Desconocido", // Asignar "Desconocido" si está vacío
                    FechaNacimiento = libroDTO.AutorFechaNacimiento // Manejar si es nulo en la clase Autor
                };
                _context.Autores.Add(autor);
                await _context.SaveChangesAsync();
            }

            libroDB.Titulo = libroDTO.Titulo;
            libroDB.Descripcion = libroDTO.Descripcion;
            libroDB.FechaDePublicacion = libroDTO.FechaDePublicacion;
            libroDB.AutorId = autor.IdAutor;

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
