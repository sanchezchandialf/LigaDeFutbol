using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Encuentro
{
    public int IdEncuentro { get; set; }

    public int? IdCancha { get; set; }

    public DateTime? FechaHora { get; set; }

    public int? GolesEquipo1 { get; set; }

    public int? GolesEquipo2 { get; set; }

    public int IdFecha { get; set; }

    public string? DniArbitro { get; set; }

    public virtual Arbitro? DniArbitroNavigation { get; set; }

    public virtual Cancha? IdCanchaNavigation { get; set; }

    public virtual Fecha IdFechaNavigation { get; set; } = null!;

    public virtual ICollection<Juega> Juegas { get; set; } = new List<Juega>();

    public virtual ICollection<Cruce> Cruces { get; set; } = new List<Cruce>();
}
