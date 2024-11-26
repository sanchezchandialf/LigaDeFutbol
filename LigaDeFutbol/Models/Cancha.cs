using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Cancha
{
    public int IdCancha { get; set; }

    public string? Nombre { get; set; }

    public string? Calle { get; set; }

    public int? Numero { get; set; }

    public string? Ciudad { get; set; }

    public virtual ICollection<Encuentro> Encuentros { get; set; } = new List<Encuentro>();
}
