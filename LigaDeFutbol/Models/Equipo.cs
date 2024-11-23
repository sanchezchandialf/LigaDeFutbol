using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Equipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdDirectorTecnico { get; set; }

    public int IdRepresentanteEquipo { get; set; }

    public virtual ICollection<EquipoFecha> EquipoFechaIdEquipo1Navigations { get; set; } = new List<EquipoFecha>();

    public virtual ICollection<EquipoFecha> EquipoFechaIdEquipo2Navigations { get; set; } = new List<EquipoFecha>();

    public virtual Persona IdDirectorTecnicoNavigation { get; set; } = null!;

    public virtual Persona IdRepresentanteEquipoNavigation { get; set; } = null!;

    public virtual ICollection<JugadorEquipo> JugadorEquipos { get; set; } = new List<JugadorEquipo>();
}
