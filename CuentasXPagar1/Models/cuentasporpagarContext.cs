using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CuentasXPagar1.Models
{
    public partial class cuentasporpagarContext : DbContext
    {
        public cuentasporpagarContext()
        {
        }

        public cuentasporpagarContext(DbContextOptions<cuentasporpagarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Conceptodepago> Conceptodepagos { get; set; }
        public virtual DbSet<Entradadocumento> Entradadocumentos { get; set; }
        public virtual DbSet<Proveedore> Proveedores { get; set; }
        public virtual DbSet<Transaccione> Transacciones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;user=root;password=12345678;database=cuentasporpagar");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conceptodepago>(entity =>
            {
                entity.ToTable("conceptodepago");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Entradadocumento>(entity =>
            {
                entity.HasKey(e => e.NumeroDocumento)
                    .HasName("PRIMARY");

                entity.ToTable("entradadocumentos");

                entity.HasIndex(e => e.ProveedorId, "FK_EntradaDocumentos_Proveedores_idx");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.ProveedorId).HasColumnName("ProveedorId");

                entity.HasOne(d => d.Proveedor)
                    .WithMany(p => p.Entradadocumentos)
                    .HasForeignKey(d => d.ProveedorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntradaDocumentos_Proveedores");
            });

            modelBuilder.Entity<Proveedore>(entity =>
            {
                entity.ToTable("proveedores");

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TipoPersona)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Transaccione>(entity =>
            {
                entity.ToTable("transacciones");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.CuentaCr).HasColumnName("Cuenta_CR");

                entity.Property(e => e.CuentasDb).HasColumnName("Cuentas_DB");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FechaTransaccion).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
