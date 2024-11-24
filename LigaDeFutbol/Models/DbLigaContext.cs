using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LigaDeFutbol.Models;

public partial class DbLigaContext : DbContext
{
    public DbLigaContext()
    {
    }

    public DbLigaContext(DbContextOptions<DbLigaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Divisione> Divisiones { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<EquipoFecha> EquipoFechas { get; set; }

    public virtual DbSet<Fecha> Fechas { get; set; }

    public virtual DbSet<JugadorEquipo> JugadorEquipos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Torneo> Torneos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "NOMBRE_categorias").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Divisione>(entity =>
        {
            entity.ToTable("divisiones");

            entity.HasIndex(e => e.Nombre, "NOMBRE_divisiones").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.ToTable("equipos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDirectorTecnico).HasColumnName("id_director_tecnico");
            entity.Property(e => e.IdRepresentanteEquipo).HasColumnName("id_representante_equipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdDirectorTecnicoNavigation).WithMany(p => p.EquipoIdDirectorTecnicoNavigations)
                .HasForeignKey(d => d.IdDirectorTecnico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_equipos_personas");

            entity.HasOne(d => d.IdRepresentanteEquipoNavigation).WithMany(p => p.EquipoIdRepresentanteEquipoNavigations)
                .HasForeignKey(d => d.IdRepresentanteEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_equipos_personas1");
        });

        modelBuilder.Entity<EquipoFecha>(entity =>
        {
            entity.HasKey(e => new { e.IdEquipo1, e.IdEquipo2, e.IdFecha });

            entity.ToTable("equipo_fechas");

            entity.Property(e => e.IdEquipo1).HasColumnName("id_equipo_1");
            entity.Property(e => e.IdEquipo2).HasColumnName("id_equipo_2");
            entity.Property(e => e.IdFecha).HasColumnName("id_fecha");
            entity.Property(e => e.FechaYHora).HasColumnName("fecha_y_hora");
            entity.Property(e => e.GolesEquipo1).HasColumnName("goles_equipo_1");
            entity.Property(e => e.GolesEquipo2).HasColumnName("goles_equipo_2");

            entity.HasOne(d => d.IdEquipo1Navigation).WithMany(p => p.EquipoFechaIdEquipo1Navigations)
                .HasForeignKey(d => d.IdEquipo1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_equipo_fechas_equipos");

            entity.HasOne(d => d.IdEquipo2Navigation).WithMany(p => p.EquipoFechaIdEquipo2Navigations)
                .HasForeignKey(d => d.IdEquipo2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_equipo_fechas_equipos1");

            entity.HasOne(d => d.IdFechaNavigation).WithMany(p => p.EquipoFechas)
                .HasForeignKey(d => d.IdFecha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_equipo_fechas_fechas");
        });

        modelBuilder.Entity<Fecha>(entity =>
        {
            entity.ToTable("fechas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTorneo).HasColumnName("id_torneo");
            entity.Property(e => e.Rueda).HasColumnName("rueda");

            entity.HasOne(d => d.IdTorneoNavigation).WithMany(p => p.Fechas)
                .HasForeignKey(d => d.IdTorneo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_fechas_torneos");
        });

        modelBuilder.Entity<JugadorEquipo>(entity =>
        {
            entity.HasKey(e => new { e.IdJugador, e.IdEquipo });

            entity.ToTable("jugador_equipos");

            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.Aprobado).HasColumnName("aprobado");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.JugadorEquipos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_jugador_equipos_equipos");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.JugadorEquipos)
                .HasForeignKey(d => d.IdJugador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_jugador_equipos_personas");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("personas");

            entity.HasIndex(e => e.Dni, "DNI_personas").IsUnique();

            entity.HasIndex(e => e.NSocio, "N_SOCIO_personas").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.Dni)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.EsDirectorTecnico).HasColumnName("es_director_tecnico");
            entity.Property(e => e.EsEncargadoAsociacion).HasColumnName("es_encargado_asociacion");
            entity.Property(e => e.EsJugador).HasColumnName("es_jugador");
            entity.Property(e => e.EsRepresentanteEquipo).HasColumnName("es_representante_equipo");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Foto)
                .HasColumnType("text")
                .HasColumnName("foto");
            entity.Property(e => e.NSocio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("n_socio");
            entity.Property(e => e.NTelefono1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("n_telefono_1");
            entity.Property(e => e.NTelefono2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("n_telefono_2");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("numero");
        });

        modelBuilder.Entity<Torneo>(entity =>
        {
            entity.ToTable("torneos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaFinalizacion).HasColumnName("fecha_finalizacion");
            entity.Property(e => e.FechaFinalizacionInscripcion).HasColumnName("fecha_finalizacion_inscripcion");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaInicioInscripcion).HasColumnName("fecha_inicio_inscripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdDivision).HasColumnName("id_division");
            entity.Property(e => e.IdEncargadoAsociacion).HasColumnName("id_encargado_asociacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Torneos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_torneos_categorias");

            entity.HasOne(d => d.IdDivisionNavigation).WithMany(p => p.Torneos)
                .HasForeignKey(d => d.IdDivision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_torneos_divisiones");

            entity.HasOne(d => d.IdEncargadoAsociacionNavigation).WithMany(p => p.Torneos)
                .HasForeignKey(d => d.IdEncargadoAsociacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_torneos_personas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
