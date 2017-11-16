using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Table("ViajeReserva")]
    public class ViajeReserva
    {
        [Key]
        public int ViajeReservaId { get; set; }

        public int ViajeId { get; set; }
        public int ReservaId { get; set; }

        public virtual Viaje Viaje { get; set; }
        public virtual Reserva Reserva { get; set; }
    }
}