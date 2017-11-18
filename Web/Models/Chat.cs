using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Table("Chat")]
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [ForeignKey("ApplicationUserEnvia")]
        public string UsuarioEnvia { get; set; }        
        public virtual ApplicationUser ApplicationUserEnvia { get; set; }

        [ForeignKey("ApplicationUserRecibe")]
        public string UsuarioRecibe { get; set; }                
        public virtual ApplicationUser ApplicationUserRecibe { get; set; }

        public string NombreUsuarioEnvia { get; set; }
        public string NombreUsuarioRecibe { get; set; }

        [Required(ErrorMessage ="{No se admiten mensajes vacíos}")]
        [DataType(DataType.MultilineText)]
        public string Mensaje { get; set; }

        public DateTime FechaCreacion { get; set; }

    }
}