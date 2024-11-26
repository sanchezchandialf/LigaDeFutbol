namespace LigaDeFutbol.Dtos
{
    public class RegistrarEquipoDTO
    {
        public string  Nombre { get; set; } 
        public int IdDirectorTecnico { get; set; }
        public int IdRepresentanteEquipo { get; set; }
        
        public int  IdTorneo { get; set; }

    }
}
