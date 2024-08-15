using WebLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebLibrary.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Autor> Autores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Autor>(tb => {
                tb.HasKey(col => col.IdAutor);
                tb.Property(col => col.IdAutor).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Nombre).HasMaxLength(50);
                tb.Property(col => col.FechaNacimiento).IsRequired();
                tb.Property(col => col.Nacionalidad).HasMaxLength(50);
                tb.ToTable("Autores");             
            });

            modelBuilder.Entity<Libro>(tb => {
                tb.HasKey(col => col.IdLibro);
                tb.Property(col => col.IdLibro).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Titulo).HasMaxLength(50);
                tb.Property(col => col.Descripcion).IsRequired();
                tb.Property(col => col.FechaDePublicacion).IsRequired();
                tb.Property(col => col.AutorId).IsRequired();
                tb.HasOne(col => col.AutorReferencia).WithMany(a => a.LibrosReferencia)
                .HasForeignKey(col => col.AutorId);

            });
        }
    }
}
