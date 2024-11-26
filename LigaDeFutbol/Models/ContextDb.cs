using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LigaDeFutbol.Models;

public partial class ContextDb : DbContext
{
    public ContextDb()
    {
    }

    public ContextDb(DbContextOptions<ContextDb> options)
        : base(options)
    {
    }

    public virtual DbSet<Arbitro> Arbitros { get; set; }

    public virtual DbSet<Cancha> Canchas { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cruce> Cruces { get; set; }

    public virtual DbSet<Divisione> Divisiones { get; set; }

    public virtual DbSet<Encuentro> Encuentros { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<ExperienciaArbitro> ExperienciaArbitros { get; set; }

    public virtual DbSet<Fecha> Fechas { get; set; }

    public virtual DbSet<Juega> Juegas { get; set; }

    public virtual DbSet<JugadorEquipo> JugadorEquipos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Ruedum> Rueda { get; set; }

    public virtual DbSet<Torneo> Torneos { get; set; }

 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Arbitro>(entity =>
        {
            entity.HasKey(e => e.DniArbitro);

            entity.ToTable("arbitro");

            entity.Property(e => e.DniArbitro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dni_arbitro");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.EsCertificado).HasColumnName("es_certificado");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.IdExperiencia).HasColumnName("id_experiencia");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero).HasColumnName("numero");

            entity.HasOne(d => d.IdExperienciaNavigation).WithMany(p => p.Arbitros)
                .HasForeignKey(d => d.IdExperiencia)
                .HasConstraintName("FK_arbitro_experiencia_arbitro");
        });

        modelBuilder.Entity<Cancha>(entity =>
        {
            entity.HasKey(e => e.IdCancha);

            entity.ToTable("cancha");

            entity.Property(e => e.IdCancha).HasColumnName("id_cancha");
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero).HasColumnName("numero");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "NOMBRE_categorias").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EdadMaxima).HasColumnName("edad_maxima");
            entity.Property(e => e.EdadMinima).HasColumnName("edad_minima");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cruce>(entity =>
        {
            entity.HasKey(e => new { e.IdEquipoA, e.IdEquipoB });

            entity.ToTable("cruce");

            entity.Property(e => e.IdEquipoA).HasColumnName("id_equipo_a");
            entity.Property(e => e.IdEquipoB).HasColumnName("id_equipo_b");

            entity.HasOne(d => d.IdEquipoANavigation).WithMany(p => p.CruceIdEquipoANavigations)
                .HasForeignKey(d => d.IdEquipoA)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cruce_equipos");

            entity.HasOne(d => d.IdEquipoBNavigation).WithMany(p => p.CruceIdEquipoBNavigations)
                .HasForeignKey(d => d.IdEquipoB)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cruce_equipos1");

            entity.HasMany(d => d.IdEncuentros).WithMany(p => p.Cruces)
                .UsingEntity<Dictionary<string, object>>(
                    "Participa",
                    r => r.HasOne<Encuentro>().WithMany()
                        .HasForeignKey("IdEncuentro")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_participa_encuentro2"),
                    l => l.HasOne<Cruce>().WithMany()
                        .HasForeignKey("IdEquipoA", "IdEquipoB")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_participa_cruce"),
                    j =>
                    {
                        j.HasKey("IdEquipoA", "IdEquipoB", "IdEncuentro");
                        j.ToTable("participa");
                        j.IndexerProperty<int>("IdEquipoA").HasColumnName("id_equipo_a");
                        j.IndexerProperty<int>("IdEquipoB").HasColumnName("id_equipo_b");
                        j.IndexerProperty<int>("IdEncuentro").HasColumnName("id_encuentro");
                    });
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

        modelBuilder.Entity<Encuentro>(entity =>
        {
            entity.HasKey(e => e.IdEncuentro);

            entity.ToTable("encuentro");

            entity.Property(e => e.IdEncuentro).HasColumnName("id_encuentro");
            entity.Property(e => e.DniArbitro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dni_arbitro");
            entity.Property(e => e.FechaHora)
                .HasColumnType("smalldatetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.GolesEquipo1).HasColumnName("goles_equipo_1");
            entity.Property(e => e.GolesEquipo2).HasColumnName("goles_equipo_2");
            entity.Property(e => e.IdCancha).HasColumnName("id_cancha");
            entity.Property(e => e.IdFecha).HasColumnName("id_fecha");

            entity.HasOne(d => d.DniArbitroNavigation).WithMany(p => p.Encuentros)
                .HasForeignKey(d => d.DniArbitro)
                .HasConstraintName("FK_encuentro_arbitro");

            entity.HasOne(d => d.IdCanchaNavigation).WithMany(p => p.Encuentros)
                .HasForeignKey(d => d.IdCancha)
                .HasConstraintName("FK_encuentro_cancha");

            entity.HasOne(d => d.IdFechaNavigation).WithMany(p => p.Encuentros)
                .HasForeignKey(d => d.IdFecha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_encuentro_fechas");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.ToTable("equipos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDirectorTecnico).HasColumnName("id_director_tecnico");
            entity.Property(e => e.IdRepresentanteEquipo).HasColumnName("id_representante_equipo");
            entity.Property(e => e.IdTorneo).HasColumnName("id_torneo");
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

            entity.HasOne(d => d.IdTorneoNavigation).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.IdTorneo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_equipos_torneos");
        });

        modelBuilder.Entity<ExperienciaArbitro>(entity =>
        {
            entity.HasKey(e => e.IdExperiencia);

            entity.ToTable("experiencia_arbitro");

            entity.Property(e => e.IdExperiencia).HasColumnName("id_experiencia");
            entity.Property(e => e.Experiencia)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("experiencia");
        });

        modelBuilder.Entity<Fecha>(entity =>
        {
            entity.ToTable("fechas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdRueda).HasColumnName("id_rueda");

            entity.HasOne(d => d.IdRuedaNavigation).WithMany(p => p.Fechas)
                .HasForeignKey(d => d.IdRueda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_fechas_rueda");
        });

        modelBuilder.Entity<Juega>(entity =>
        {
            entity.HasKey(e => new { e.IdJugador, e.IdEncuentro });

            entity.ToTable("juega");

            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.IdEncuentro).HasColumnName("id_encuentro");
            entity.Property(e => e.Goles)
                .HasDefaultValue(0)
                .HasColumnName("goles");
            entity.Property(e => e.TarjetaAmarilla)
                .HasDefaultValue(0)
                .HasColumnName("tarjeta_amarilla");
            entity.Property(e => e.TarjetaRoja)
                .HasDefaultValue(false)
                .HasColumnName("tarjeta_roja");

            entity.HasOne(d => d.IdEncuentroNavigation).WithMany(p => p.Juegas)
                .HasForeignKey(d => d.IdEncuentro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_juega_encuentro");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.Juegas)
                .HasForeignKey(d => d.IdJugador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_juega_personas");
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
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contraseña");
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
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdDivision).HasColumnName("id_division");
            entity.Property(e => e.NSocio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("n_socio");
            entity.Property(e => e.NTelefono1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("n_telefono_1");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("numero");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_personas_categorias");

            entity.HasOne(d => d.IdDivisionNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdDivision)
                .HasConstraintName("FK_personas_divisiones");
        });

        modelBuilder.Entity<Ruedum>(entity =>
        {
            entity.HasKey(e => e.IdRueda);

            entity.ToTable("rueda");

            entity.Property(e => e.IdRueda).HasColumnName("id_rueda");
            entity.Property(e => e.IdTorneoFk).HasColumnName("id_torneo_fk");

            entity.HasOne(d => d.IdTorneoFkNavigation).WithMany(p => p.Rueda)
                .HasForeignKey(d => d.IdTorneoFk)
                .HasConstraintName("FK_rueda_torneos");
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
