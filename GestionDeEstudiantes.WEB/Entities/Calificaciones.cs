﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace GestionDeEstudiantes.WEB.Entities
{
    public partial class Calificaciones
    {
        public Calificaciones()
        {
            DetalleCalificaciones = new HashSet<DetalleCalificaciones>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public int IdMateria { get; set; }        
        public int AnoEscolar { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Calificada { get; set; }
        public DateTime? FechaCalificacion { get; set; }

        public virtual Materias IdMateriaNavigation { get; set; }
        public virtual ICollection<DetalleCalificaciones> DetalleCalificaciones { get; set; }
    }
}