using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BibliotecaUDEO.Models
{
    public partial class B1Context : DbContext
    {
        public B1Context()
        {
        }

        public B1Context(DbContextOptions<B1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Anio> Anios { get; set; }
        public virtual DbSet<Autor> Autors { get; set; }
        public virtual DbSet<AutorDocumento> AutorDocumentos { get; set; }
        public virtual DbSet<Carrera> Carreras { get; set; }
        public virtual DbSet<Categorium> Categoria { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<DocumentoItem> DocumentoItems { get; set; }
        public virtual DbSet<Editorial> Editorials { get; set; }
        public virtual DbSet<Prestamo> Prestamos { get; set; }
        public virtual DbSet<Sede> Sedes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagDocumento> TagDocumentos { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:BibliotecaDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Anio>(entity =>
            {
                entity.ToTable("Anio");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("Autor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("fecha_nacimiento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<AutorDocumento>(entity =>
            {
                entity.ToTable("AutorDocumento");

                entity.HasIndex(e => e.DocumentoId, "fkIdx_118");

                entity.HasIndex(e => e.AutorId, "fkIdx_121");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AutorId).HasColumnName("autor_id");

                entity.Property(e => e.DocumentoId).HasColumnName("documento_id");

                entity.HasOne(d => d.Autor)
                    .WithMany(p => p.AutorDocumentos)
                    .HasForeignKey(d => d.AutorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_120");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.AutorDocumentos)
                    .HasForeignKey(d => d.DocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_117");
            });

            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.ToTable("Carrera");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.ToTable("Division");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.ToTable("Documento");

                entity.HasIndex(e => e.EditorialId, "fkIdx_186");

                entity.HasIndex(e => e.AnioId, "fkIdx_22");

                entity.HasIndex(e => e.CategoriaId, "fkIdx_29");

                entity.HasIndex(e => e.DivisionId, "fkIdx_36");

                entity.HasIndex(e => e.TipoDocumentoId, "fkIdx_47");

                entity.HasIndex(e => e.CarreraId, "fkIdx_54");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AnioId).HasColumnName("anio_id");

                entity.Property(e => e.CarreraId).HasColumnName("carrera_id");

                entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("codigo");

                entity.Property(e => e.Creado)
                    .HasColumnType("datetime")
                    .HasColumnName("creado");

                entity.Property(e => e.DivisionId).HasColumnName("division_id");

                entity.Property(e => e.EditorialId).HasColumnName("editorial_id");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("image");

                entity.Property(e => e.Modificado)
                    .HasColumnType("datetime")
                    .HasColumnName("modificado");

                entity.Property(e => e.TipoDocumentoId).HasColumnName("tipo_documento_id");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.Anio)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.AnioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_21");

                entity.HasOne(d => d.Carrera)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.CarreraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_53");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_28");

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_35");

                entity.HasOne(d => d.Editorial)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.EditorialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_185");

                entity.HasOne(d => d.TipoDocumento)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.TipoDocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_46");
            });

            modelBuilder.Entity<DocumentoItem>(entity =>
            {
                entity.ToTable("DocumentoItem");

                entity.HasIndex(e => e.SedeId, "fkIdx_124");

                entity.HasIndex(e => e.DocumentoId, "fkIdx_72");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.DocumentoId).HasColumnName("documento_id");

                entity.Property(e => e.EsFisico).HasColumnName("es_fisico");

                entity.Property(e => e.LibroUrl)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("libro_url");

                entity.Property(e => e.NumeroPrestamos).HasColumnName("numero_prestamos");

                entity.Property(e => e.SedeId).HasColumnName("sede_id");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.DocumentoItems)
                    .HasForeignKey(d => d.DocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_71");

                entity.HasOne(d => d.Sede)
                    .WithMany(p => p.DocumentoItems)
                    .HasForeignKey(d => d.SedeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_123");
            });

            modelBuilder.Entity<Editorial>(entity =>
            {
                entity.ToTable("Editorial");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("Prestamo");

                entity.HasIndex(e => e.DocumentoItemId, "fkIdx_84");

                entity.HasIndex(e => e.UsuarioId, "fkIdx_94");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Danio).HasColumnName("danio");

                entity.Property(e => e.Devuelto).HasColumnName("devuelto");

                entity.Property(e => e.DiasAtraso).HasColumnName("dias_atraso");

                entity.Property(e => e.DocumentoItemId).HasColumnName("documento_item_id");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("date")
                    .HasColumnName("fecha_fin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("fecha_inicio");

                entity.Property(e => e.Perdida).HasColumnName("perdida");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.HasOne(d => d.DocumentoItem)
                    .WithMany(p => p.Prestamos)
                    .HasForeignKey(d => d.DocumentoItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_83");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Prestamos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_93");
            });

            modelBuilder.Entity<Sede>(entity =>
            {
                entity.ToTable("Sede");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("direccion");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<TagDocumento>(entity =>
            {
                entity.ToTable("tag_documento");

                entity.HasIndex(e => e.DocumentoId, "fkIdx_109");

                entity.HasIndex(e => e.TagId, "fkIdx_112");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DocumentoId).HasColumnName("documento_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.TagDocumentos)
                    .HasForeignKey(d => d.DocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_108");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagDocumentos)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_111");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("apellido");

                entity.Property(e => e.GoogleId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("google_id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("nombre");

                entity.Property(e => e.Rol)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
