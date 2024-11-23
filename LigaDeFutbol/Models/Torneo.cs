using System;
using System.Collections.Generic;

namespace LigaDeFutbol.Models;

public partial class Torneo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFinalizacion { get; set; }

    public DateOnly FechaInicioInscripcion { get; set; }

    public DateOnly FechaFinalizacionInscripcion { get; set; }

    public int IdEncargadoAsociacion { get; set; }

    public int IdCategoria { get; set; }

    public int IdDivision { get; set; }

    public virtual ICollection<Fecha> Fechas { get; set; } = new List<Fecha>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Divisione IdDivisionNavigation { get; set; } = null!;

    public virtual Persona IdEncargadoAsociacionNavigation { get; set; } = null!;
}
