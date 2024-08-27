using Microsoft.EntityFrameworkCore;
using WebLibrary.Context;
using WebLibrary.DTOs;
using WebLibrary.Entities;

namespace WebLibrary.Services
{
    public class AutorService
    {
        private readonly AppDbContext _context;

        public AutorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutorDTO>> Lista()
        {
            var listaDTO = new List<AutorDTO>();

            foreach (var item in await _context.Autores.ToListAsync())
            {
                listaDTO.Add(new AutorDTO
                {
                    IdAutor = item.IdAutor,
                    Nombre = item.Nombre,
                    FechaNacimiento = item.FechaNacimiento,
                    Nacionalidad = item.Nacionalidad
                });
            }
            return listaDTO;
        }

        public async Task<AutorDTO> GetAutorByIdAsync(int id)
        {
            var autorDB = await _context.Autores.FindAsync(id);
            if (autorDB == null)
            {
                return null;
            }

            return new AutorDTO
            {
                IdAutor = autorDB.IdAutor,
                Nombre = autorDB.Nombre,
                FechaNacimiento = autorDB.FechaNacimiento,
                Nacionalidad = autorDB.Nacionalidad
            };
        }

        public async Task<AutorDTO> GetAutorByNombreAsync(string nombre)
        {
            var autorDB = await _context.Autores.FirstOrDefaultAsync(a => a.Nombre == nombre);
            if (autorDB == null)
            {
                return null;
            }

            return new AutorDTO
            {
                IdAutor = autorDB.IdAutor,
                Nombre = autorDB.Nombre,
                FechaNacimiento = autorDB.FechaNacimiento,
                Nacionalidad = autorDB.Nacionalidad
            };
        }


        public async Task CrearAutorAsync(AutorDTO autorDTO)
        {
            var autorDB = new Autor
            {
                Nombre = autorDTO.Nombre,
                FechaNacimiento = autorDTO.FechaNacimiento,
                Nacionalidad = autorDTO.Nacionalidad
            };
            _context.Autores.Add(autorDB);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditarAutorAsync(AutorDTO autorDTO)
        {
            var autorDB = await _context.Autores.FindAsync(autorDTO.IdAutor);
            if (autorDB == null)
            {
                return false;
            }

            autorDB.Nombre = autorDTO.Nombre;
            autorDB.FechaNacimiento = autorDTO.FechaNacimiento;
            autorDB.Nacionalidad = autorDTO.Nacionalidad;

            _context.Autores.Update(autorDB);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAutorAsync(int id)
        {
            var autorDB = await _context.Autores.FindAsync(id);
            if (autorDB == null)
            {
                return false;
            }
            _context.Autores.Remove(autorDB);
            await _context.SaveChangesAsync();
            return true;
        }         



        public async Task<List<LibroDTO>> GetLibrosByAutorNombreAsync(string nombre)
        {
            var autor = await _context.Autores
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.Nombre == nombre);

            if (autor == null)
            {
                return null;
            }

            var librosDTO = autor.Libros.Select(libro => new LibroDTO
            {
                IdLibro = libro.IdLibro,
                Titulo = libro.Titulo,
                Descripcion = libro.Descripcion,
                FechaDePublicacion = libro.FechaDePublicacion,
                AutorId = libro.AutorId
            }).ToList();

            return librosDTO;
        }
    }
}
