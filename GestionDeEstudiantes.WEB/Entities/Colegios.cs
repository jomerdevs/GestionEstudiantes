﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace GestionDeEstudiantes.WEB.Entities
{
    public partial class Colegios
    {
        public Colegios()
        {
            Estudiantes = new HashSet<Estudiantes>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Description { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Estudiantes> Estudiantes { get; set; }
    }
}