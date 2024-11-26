using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Ruedum
{
    public int IdRueda { get; set; }

    public int? IdTorneoFk { get; set; }

    public virtual ICollection<Fecha> Fechas { get; set; } = new List<Fecha>();

    public virtual Torneo? IdTorneoFkNavigation { get; set; }
}
