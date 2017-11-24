using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Web.Models.ViewModels
{
    public class BuscarViaje
    {
        private ApplicationDbContext db;

        public BuscarViaje()
        {
            db = new ApplicationDbContext();
        }

        [Display(Name = "Punto de Partida")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string PuntoPartida { get; set; }

        [Display(Name = "Punto de Destino")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string PuntoDestino { get; set; }

        [DataType(DataType.Date)]
        public string Fecha { get; set; }

        public List<Viaje> listaViajesBuscados(BuscarViaje model)
        {
            var context = new IdentityDbContext();
            var allUsers = context.Users.ToList();
            //foreach( var item in allUsers)
            //{
            //    Console.WriteLine(item.Id+" "+item.UserName);
            //}

            //var viajes = (from v in db.Viajes                          
            //              where (v.PuntoPartida.Contains(model.PuntoPartida))
            //                    ||
            //                    (v.PuntoPartida.Contains(model.PuntoDestino))                          
            //              select v).ToList();

            //var viajes = (from Viajes in db.Viajes
            //              join Usuarios in allUsers
            //              on Viajes.ApplicationUserId equals Usuarios.Id                                                   
            //              select Viajes).ToList();

            var viajes = db.Viajes
                        .Include("ApplicationUser")
                        .Where(v => v.PuntoPartida.Contains(model.PuntoPartida))
                        .Where(v => v.PuntoDestino.Contains(model.PuntoDestino))                        
                        .ToList();

           foreach(var item in viajes)
            {
                Console.WriteLine(item.ApplicationUser.Id+" "+item.ApplicationUser.UserName+" "+item.PuntoPartida);
            }

            return viajes;
        }
    }
}