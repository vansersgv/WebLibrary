using System;
using System.ComponentModel.DataAnnotations;

namespace WebLibrary.DTOs
{
    public class AutorDTO
    {
        public int IdAutor { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(100, ErrorMessage = "La nacionalidad no puede tener más de 100 caracteres")]
        public string? Nacionalidad { get; set; }
    }
}
