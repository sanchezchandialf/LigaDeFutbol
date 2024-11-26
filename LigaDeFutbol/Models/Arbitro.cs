using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Arbitro
{
    public string DniArbitro { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Calle { get; set; }

    public int? Numero { get; set; }

    public string? Ciudad { get; set; }

    public bool? EsCertificado { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public int? IdExperiencia { get; set; }

    public virtual ICollection<Encuentro> Encuentros { get; set; } = new List<Encuentro>();

    public virtual ExperienciaArbitro? IdExperienciaNavigation { get; set; }
}
