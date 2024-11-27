namespace LigaDeFutbol.Dtos
{
    public class CrearTorneoDTO
    {
        public string Nombre { get; set; } = null!;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFinalizacion { get; set; }
        public DateOnly FechaInicioInscripcion { get; set; }
        public DateOnly FechaFinalizacionInscripcion { get; set; }
        public int IdEncargadoAsociacion { get; set; }
        public int IdCategoria { get; set; }
        public int IdDivision { get; set; }
    }
}
