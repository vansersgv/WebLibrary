﻿namespace WebLibrary.Entities
{
    public class Autor
    {
        public int IdAutor { get; set; }
        public required string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Nacionalidad { get; set; }
        public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
