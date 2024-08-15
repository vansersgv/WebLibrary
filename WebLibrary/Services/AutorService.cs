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
            _context=context;
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

    }
}
