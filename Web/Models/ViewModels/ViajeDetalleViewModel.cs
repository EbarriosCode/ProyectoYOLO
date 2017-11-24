using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModels
{
    public class ViajeDetalleViewModel
    {
        public int ViajeId { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApplicationUserNombrePiloto { get; set; }
        public string ApplicationUserApellidoPiloto { get; set; }
        public int ApplicationUserEdad{ get; set; }
        public string PuntoPartida { get; set; }
        public string PuntoDestino { get; set; }
        public string FechaSalida { get; set; }
        public string HoraSalida { get; set; }
        public string FechaRegreso { get; set; }
        public string HoraRegreso { get; set; }
        public string Distancia { get; set; }
        public string Duracion { get; set; }
        public double Tarifa { get; set; }        
        public int NumAsientos { get; set; }        
        public string DetallesViaje { get; set; }

        public List<ViajeReserva> Pasajeros { get; set; }
    }
}