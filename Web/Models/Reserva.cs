using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Table("Reserva")]
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        //Relacion del usuario en una reserva
        [Display(Name ="Pasajero Id")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        
        [Display(Name = "Número de Asientos")]
        public int NumAsientos { get; set; }

        public bool Aceptado { get; set; }

        //Propiedad para la relacion de la tabla intermedia de muchos a muchos Viaje/Reserva
        public virtual ICollection<ViajeReserva> ViajesReservas { get; set; }
    }
}