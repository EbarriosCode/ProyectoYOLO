using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class MisReservasViewModel
    {
        public string NombreConductor { get; set; }
        public string ApellidoConductor { get; set; }
        public string TelefonoConductor { get; set; }
        public string PuntoSalida { get; set; }
        public string PuntoDestino { get; set; }        
        public string FechaSalida { get; set; }        
        public string HoraSalida { get; set; }        
        public string FechaRegreso { get; set; }
        public string HoraRegreso { get; set; }
        public string Distancia { get; set; }
        public string Duracion { get; set; }
        public double Tarifa { get; set; }
        public int NumAsientosReservados { get; set; }

    }
}