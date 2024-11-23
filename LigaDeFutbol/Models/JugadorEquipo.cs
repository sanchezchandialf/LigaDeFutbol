using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class JugadorEquipo
{
    public int IdJugador { get; set; }

    public int IdEquipo { get; set; }

    public bool Aprobado { get; set; }

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual Persona IdJugadorNavigation { get; set; } = null!;
}
