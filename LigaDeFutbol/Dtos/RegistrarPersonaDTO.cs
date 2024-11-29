namespace LigaDeFutbol.DTos
{
    public class RegistrarPersonaDTO
    {
        public string? Foto { get; set; } = null!;
        public string Dni { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Calle { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string NTelefono1 { get; set; }= null!;
        public string? NTelefono2 { get; set; }
        public string Contraseña { get; set; } = null!;
        public bool EsJugador { get; set; }
        public bool EsRepresentanteEquipo { get; set; } 
        public bool EsDirectorTecnico { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdDivision { get; set; }
    }
}
