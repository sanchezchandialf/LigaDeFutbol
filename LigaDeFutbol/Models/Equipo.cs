using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Equipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdDirectorTecnico { get; set; }

    public int IdRepresentanteEquipo { get; set; }

    public int IdTorneo { get; set; }

    public virtual ICollection<Cruce> CruceIdEquipoANavigations { get; set; } = new List<Cruce>();

    public virtual ICollection<Cruce> CruceIdEquipoBNavigations { get; set; } = new List<Cruce>();

    public virtual Persona IdDirectorTecnicoNavigation { get; set; } = null!;

    public virtual Persona IdRepresentanteEquipoNavigation { get; set; } = null!;

    public virtual Torneo IdTorneoNavigation { get; set; } = null!;

    public virtual ICollection<JugadorEquipo> JugadorEquipos { get; set; } = new List<JugadorEquipo>();
}
