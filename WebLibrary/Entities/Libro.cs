namespace WebLibrary.Entities
{
    public class Libro
    {
        public int IdLibro { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaDePublicacion { get; set; }
        public int AutorId { get; set; }
        public virtual Autor Autor { get; set; }
    }

}
