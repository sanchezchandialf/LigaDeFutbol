using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class EquipoFecha
{
    public int IdEquipo1 { get; set; }

    public int IdEquipo2 { get; set; }

    public int IdFecha { get; set; }

    public int? GolesEquipo1 { get; set; }

    public int? GolesEquipo2 { get; set; }

    public DateTime FechaYHora { get; set; }

    public virtual Equipo IdEquipo1Navigation { get; set; } = null!;

    public virtual Equipo IdEquipo2Navigation { get; set; } = null!;

    public virtual Fecha IdFechaNavigation { get; set; } = null!;
}
