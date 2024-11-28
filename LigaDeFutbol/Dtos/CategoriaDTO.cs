namespace LigaDeFutbol.Models.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public int EdadMinima { get; set; }
        public int EdadMaxima { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
