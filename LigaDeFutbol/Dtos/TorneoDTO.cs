using LigaDeFutbol.Models.DTOs;

namespace LigaDeFutbol.Dtos
{
    public class TorneoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFinalizacion { get; set; }

        public DateOnly FechaInicioInscripcion { get; set; }
        public DateOnly FechaFinalizacionInscripcion { get; set; }

        public int IdDivision { get; set; }
        public int IdCategoria { get; set; }
        public DivisionDTO? Division { get; set; }
        public CategoriaDTO? Categoria { get; set; }
    }


}
