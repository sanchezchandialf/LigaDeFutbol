namespace LigaDeFutbol.Dtos
{
    public class PersonaDTO
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Ciudad { get; set; }
        public string NTelefono1 { get; set; }
        public bool EsJugador { get; set; }
        public bool EsDirectorTecnico { get; set; }
        public bool EsRepresentanteEquipo { get; set; }
        public bool EsRepresentanteAsociacion { get; set; }

        public string? Foto { get; set; }

        public string? NSocio { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdDivision { get; set; }
    }
}
