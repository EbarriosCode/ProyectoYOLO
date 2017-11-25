using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Table("Conversacion")]
    public class Conversacion
    {
        [Key]
        public int ConversacionId { get; set; }

        public string ApplicationUserId { get; set; }
        public int MensajeId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Mensajes Mensaje { get; set; }
    }
}