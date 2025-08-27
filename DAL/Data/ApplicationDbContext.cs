using System;
using System.Collections.Generic;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradores { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<EstadoTurno> EstadoTurnos { get; set; }

    public virtual DbSet<HistorialTurno> HistorialTurnos { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.AdministradorId).HasName("PK__Administ__2C780D768C04393D");

            entity.HasIndex(e => e.Correo, "UQ__Administ__60695A193AB18BE5").IsUnique();

            entity.HasIndex(e => e.Usuario, "UQ__Administ__E3237CF73B24F282").IsUnique();

            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD08713A28857");

            entity.HasIndex(e => e.ClienteId, "idx_clienteId");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NroCelular)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoTurno>(entity =>
        {
            entity.HasKey(e => e.EstadoTurnoId).HasName("PK__EstadoTu__10C65F06EAEE620F");

            entity.ToTable("EstadoTurno");

            entity.HasIndex(e => e.EstadoTurnoId, "idx_estadoTurnoId");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HistorialTurno>(entity =>
        {
            entity.HasKey(e => e.HistorialTurnoId).HasName("PK__Historia__1CF9757939A17C0F");

            entity.ToTable("HistorialTurno");

            entity.HasOne(d => d.Administrador).WithMany(p => p.HistorialTurnos)
                .HasForeignKey(d => d.AdministradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialTurno_Administrador");

            entity.HasOne(d => d.Turno).WithMany(p => p.HistorialTurnos)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Turno");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.MetodoPagoId).HasName("PK__MetodoPa__A8FEAF5471A25E67");

            entity.ToTable("MetodoPago");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEECC205ED5477");

            entity.ToTable("Servicio");

            entity.HasIndex(e => e.ServicioId, "idx_servicioId");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.TipoServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_TipoServicio");
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.TipoServicioId).HasName("PK__TipoServ__BC9FF47D782EAA7F");

            entity.ToTable("TipoServicio");

            entity.HasIndex(e => e.TipoServicioId, "idx_tipoServicioId");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.TurnoId).HasName("PK__Turno__AD3E2E9443AF8BC7");

            entity.ToTable("Turno");

            entity.HasIndex(e => e.TurnoId, "idx_turnoId");

            entity.Property(e => e.Detalle)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Administrador).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.AdministradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_Administradores");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_Cliente");

            entity.HasOne(d => d.EstadoTurno).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.EstadoTurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_EstadoTurno");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_MetodoPago");

            entity.HasOne(d => d.Servicio).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_Servicio");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
