using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Fecha
{
    public int Id { get; set; }

    public int IdRueda { get; set; }

    public virtual ICollection<Encuentro> Encuentros { get; set; } = new List<Encuentro>();

    public virtual Ruedum IdRuedaNavigation { get; set; } = null!;
}
