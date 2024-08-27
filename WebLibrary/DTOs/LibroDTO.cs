using System;
using System.ComponentModel.DataAnnotations;

namespace WebLibrary.DTOs
{
    public class LibroDTO
    {
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede tener más de 200 caracteres")]
        public string? Titulo { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede tener más de 1000 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de publicación es obligatoria")]
        public DateTime FechaDePublicacion { get; set; }

        [Required(ErrorMessage = "El ID del autor es obligatorio")]
        public int AutorId { get; set; }

        public string? AutorNombre { get; set; } // Agregar esta propiedad
        public string? AutorNacionalidad { get; set; } // Agregar esta propiedad
        [Required(ErrorMessage = "La fecha de nacimiento del autor es obligatoria")]
        public DateTime AutorFechaNacimiento { get; set; }
    }
}
