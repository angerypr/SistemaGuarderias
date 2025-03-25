using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGuarderias.Domain.Entities;

namespace SistemaGuarderias.Infrastructure
{
    public class SistemaGuarderiasDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-H3L6RGC;Database=SistemaGuarderiasDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
        public SistemaGuarderiasDbContext(DbContextOptions<SistemaGuarderiasDbContext> options) : base(options) { }

        public SistemaGuarderiasDbContext() { }
        public DbSet<Guarderia> Guarderias { get; set; }
        public DbSet<Nino> Ninos { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relaciones
            modelBuilder.Entity<Nino>()
                .HasOne(n => n.Guarderia)
                .WithMany(g => g.Ninos)
                .HasForeignKey(n => n.GuarderiaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Nino>()
                .HasOne(n => n.Tutor)
                .WithMany(t => t.Ninos)
                .HasForeignKey(n => n.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Guarderia)
                .WithMany(g => g.Empleados)
                .HasForeignKey(e => e.GuarderiaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.Nino)
                .WithMany(n => n.Asistencias)
                .HasForeignKey(a => a.NinoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Asistencia>()
                .HasOne(a => a.Guarderia)
                .WithMany(g => g.Asistencias)
                .HasForeignKey(a => a.GuarderiaId)
                .OnDelete(DeleteBehavior.Restrict);

            //Configuracion Nino
            modelBuilder.Entity<Nino>(entity =>
            {
                entity.Property(n => n.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(n => n.Apellido)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(n => n.Edad)
                    .IsRequired();

                entity.HasOne(n => n.Guarderia)
                    .WithMany(g => g.Ninos)
                    .HasForeignKey(n => n.GuarderiaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Tutor)
                    .WithMany(t => t.Ninos)
                    .HasForeignKey(n => n.TutorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(n => n.Nombre);
            });


            //Configuracion asistencia 
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.Property(a => a.Fecha)
                    .IsRequired();

                entity.Property(a => a.Presente) 
                    .IsRequired();

                entity.HasOne(a => a.Nino)
                    .WithMany(n => n.Asistencias)
                    .HasForeignKey(a => a.NinoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Guarderia) 
                    .WithMany(g => g.Asistencias)
                    .HasForeignKey(a => a.GuarderiaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => new { a.NinoId, a.Fecha })
                    .IsUnique();
            });

            //Configuracion Empleado
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Cargo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15);

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(e => e.Guarderia)
                    .WithMany(g => g.Empleados)
                    .HasForeignKey(e => e.GuarderiaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Cedula)
                    .IsUnique();

                entity.HasIndex(e => e.CorreoElectronico)
                    .IsUnique();
            });

            //Configuracion Guarderia
            modelBuilder.Entity<Guarderia>(entity =>
            {
                entity.Property(g => g.Nombre)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(g => g.Direccion)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasIndex(g => g.Nombre)
                    .IsUnique();
            });


            //Indices
            modelBuilder.Entity<Nino>()
                 .HasIndex(n => n.Nombre);

            modelBuilder.Entity<Tutor>()
                .HasIndex(t => t.CorreoElectronico)
                .IsUnique();

            modelBuilder.Entity<Tutor>()
                .HasIndex(t => t.Cedula)
                .IsUnique();

            modelBuilder.Entity<Guarderia>()
                .HasIndex(g => g.Nombre)
                .IsUnique();

            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Nombre);
        }

    }
}