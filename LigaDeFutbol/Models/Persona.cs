using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string? Foto { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Calle { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string NTelefono1 { get; set; } = null!;

    public bool EsEncargadoAsociacion { get; set; }

    public bool EsDirectorTecnico { get; set; }

    public bool EsRepresentanteEquipo { get; set; }

    public bool EsJugador { get; set; }

    public string? NSocio { get; set; }

    public int? IdCategoria { get; set; }

    public int? IdDivision { get; set; }

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Equipo> EquipoIdDirectorTecnicoNavigations { get; set; } = new List<Equipo>();

    public virtual ICollection<Equipo> EquipoIdRepresentanteEquipoNavigations { get; set; } = new List<Equipo>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual Divisione? IdDivisionNavigation { get; set; }

    public virtual ICollection<Juega> Juegas { get; set; } = new List<Juega>();

    public virtual ICollection<JugadorEquipo> JugadorEquipos { get; set; } = new List<JugadorEquipo>();

    public virtual ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
}
