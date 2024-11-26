using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class ExperienciaArbitro
{
    public string? Experiencia { get; set; }

    public int IdExperiencia { get; set; }

    public virtual ICollection<Arbitro> Arbitros { get; set; } = new List<Arbitro>();
}
