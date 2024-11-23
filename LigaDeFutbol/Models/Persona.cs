using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string Foto { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Calle { get; set; } = null!;

    public string Numero { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string NTelefono1 { get; set; } = null!;

    public string? NTelefono2 { get; set; }

    public bool EsEncargadoAsociacion { get; set; }

    public bool EsDirectorTecnico { get; set; }

    public bool EsRepresentanteEquipo { get; set; }

    public bool EsJugador { get; set; }

    public string? NSocio { get; set; }

    public virtual ICollection<Equipo> EquipoIdDirectorTecnicoNavigations { get; set; } = new List<Equipo>();

    public virtual ICollection<Equipo> EquipoIdRepresentanteEquipoNavigations { get; set; } = new List<Equipo>();

    public virtual ICollection<JugadorEquipo> JugadorEquipos { get; set; } = new List<JugadorEquipo>();

    public virtual ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
}
