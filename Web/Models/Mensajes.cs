using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Table("Mensaje")]
    public class Mensajes
    {
        [Key]
        public int MensajeId { get; set; }

        [ForeignKey("ApplicationUserEnvia")]
        public string UsuarioEnvia { get; set; }        
        public virtual ApplicationUser ApplicationUserEnvia { get; set; }

        [ForeignKey("ApplicationUserRecibe")]
        public string UsuarioRecibe { get; set; }                
        public virtual ApplicationUser ApplicationUserRecibe { get; set; }

        [Required(ErrorMessage ="{No se admiten mensajes vacíos}")]
        [DataType(DataType.MultilineText)]
        public string Mensaje { get; set; }

        public DateTime FechaCreacion { get; set; }

        //Propiedad virtual para navegar entre conversaciones
        public virtual ICollection<Conversacion> Conversaciones { get; set; }

    }
}