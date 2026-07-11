using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Joseco.DDD.Core.Abstractions;

namespace GestionClinicaNutricional.Domain
{
    public class Paciente: AggregateRoot
    {
        [Key]
        public Guid Id { get; set; }
        public string CI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        
        public virtual ICollection<ConsultaInicial> ConsultaIniciales { get; set; } = new List<ConsultaInicial>();
        // public Guid ConsultaInicialId { get; set; }
        // public virtual ConsultaInicial ConsultaInicial { get; set; }
        
        
        [JsonIgnore] public virtual ICollection<PlanAlimenticio> PlanAlimenticios { get; set; }
    }
}