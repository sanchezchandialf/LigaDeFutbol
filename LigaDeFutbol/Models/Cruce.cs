using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Cruce
{
    public int IdEquipoA { get; set; }

    public int IdEquipoB { get; set; }

    public virtual Equipo IdEquipoANavigation { get; set; } = null!;

    public virtual Equipo IdEquipoBNavigation { get; set; } = null!;

    public virtual ICollection<Encuentro> IdEncuentros { get; set; } = new List<Encuentro>();
}
