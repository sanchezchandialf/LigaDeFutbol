namespace LigaDeFutbol.Dtos
{
    public class RegistrarEquipoDTO
    {
        public string Nombre { get; set; } = null!;
        public int IdDirectorTecnico { get; set; }
        public int IdRepresentanteEquipo { get; set; }
    }
}
