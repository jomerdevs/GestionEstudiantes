// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace GestionDeEstudiantes.WEB.Entities
{
    public partial class Materias
    {
        public Materias()
        {
            Calificaciones = new HashSet<Calificaciones>();
        }

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Description { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual Usuarios IdUsuarioNavigation { get; set; }
        public virtual ICollection<Calificaciones> Calificaciones { get; set; }
    }
}