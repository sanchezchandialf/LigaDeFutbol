namespace LigaDeFutbol.Dtos
{
    public class CrearTorneoDTO
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public DateTime FechaInicioInscripcion { get; set; }
        public DateTime FechaFinalizacionInscripcion { get; set; }
        public int IdEncargadoAsociacion { get; set; }
        public int IdCategoria { get; set; }
        public int IdDivision { get; set; }
        public int Ruedas { get; set; }
    }
}
