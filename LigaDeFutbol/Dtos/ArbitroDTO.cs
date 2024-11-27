namespace LigaDeFutbol.DTOs
{
    public class ArbitroDto
    {
        public string DniArbitro { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Ciudad { get; set; }

        public bool? EsCertificado { get; set; }

        public int? IdExperiencia { get; set; }
    }
}
