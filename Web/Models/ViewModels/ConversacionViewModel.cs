using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class ConversacionViewModel
    {
        public string NombreUserEnvia { get; set; }
        public string NombreUserRecibe { get; set; }
        public string ListaMensajes { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}