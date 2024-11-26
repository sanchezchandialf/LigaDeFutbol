using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Divisione
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();

    public virtual ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
}
