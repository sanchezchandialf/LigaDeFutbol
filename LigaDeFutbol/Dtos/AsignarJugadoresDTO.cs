namespace LigaDeFutbol.Dtos
{
    public class AsignarJugadoresDTO
    {
        public int IdEquipo { get; set; }
        public List<int> IdJugadores { get; set; } = new List<int>();
    }
}
