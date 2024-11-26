using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Juega
{
    public int IdJugador { get; set; }

    public int IdEncuentro { get; set; }

    public bool? TarjetaRoja { get; set; }

    public int? TarjetaAmarilla { get; set; }

    public int? Goles { get; set; }

    public virtual Encuentro IdEncuentroNavigation { get; set; } = null!;

    public virtual Persona IdJugadorNavigation { get; set; } = null!;
}
