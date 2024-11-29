namespace LigaDeFutbol.Dtos
{
    public class EquipoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdDirectorTecnico { get; set; }
        public int IdRepresentanteEquipo { get; set; }
        public int IdTorneo { get; set; }

        public TorneoDTO Torneo { get; set; }
        public PersonaDTO DirectorTecnico { get; set; }
        public PersonaDTO RepresentanteEquipo { get; set; }
    }
}
