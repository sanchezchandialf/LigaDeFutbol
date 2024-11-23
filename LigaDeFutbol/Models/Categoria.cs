﻿using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Torneo> Torneos { get; set; } = new List<Torneo>();
}
