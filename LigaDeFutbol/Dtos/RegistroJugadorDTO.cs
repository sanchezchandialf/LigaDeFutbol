namespace LigaDeFutbol.DTos
{
    public class RegistroJugadorDTO
    {
        public string Foto { get; set; } = null!;
        public string Dni { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Calle { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string NTelefono1 { get; set; } = null!;
        public string? NTelefono2 { get; set; }
    }
}
