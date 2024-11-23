using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Fecha
{
    public int Id { get; set; }

    public int Rueda { get; set; }

    public int IdTorneo { get; set; }

    public virtual ICollection<EquipoFecha> EquipoFechas { get; set; } = new List<EquipoFecha>();

    public virtual Torneo IdTorneoNavigation { get; set; } = null!;
}
