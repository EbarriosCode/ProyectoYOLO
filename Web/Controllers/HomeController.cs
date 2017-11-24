using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using Web.Models;
using Web.Models.ViewModels;

namespace Web.Controllers
{
   
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {                        
            return View();
        }

        [Authorize]
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact(int? page)
        {
            ViewBag.Message = "Your contact page.";

            var lista = db.Viajes.ToList();
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            return View(lista.ToPagedList(pageNumber,pageSize));
        }

        

        [HttpPost]
        public ActionResult BuscarViajes(BuscarViaje model, int? page)
        {
            

            //Obteniendo el listado de la tabla AspNetUsers
            var ctx = Request.GetOwinContext().Get<ApplicationDbContext>();
            //var users = ctx.Users;
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(ctx));
            var users = manager.Users.ToList();

            

            BuscarViaje viewModel = new BuscarViaje();
            List<Viaje> listaViajes = viewModel.listaViajesBuscados(model);

            //int pageSize = 2;
            //int pageNumber = (page ?? 1);

            //PagedList<Viaje> modelPaginado = new PagedList<Viaje>(listaViajes, pageNumber, pageSize);
            
            return View("BusquedaViajes", listaViajes/*.ToPagedList(pageNumber,pageSize)*/);
        }

        //Debe estar logueado para acceder a este metodo
        [Authorize]
        [HttpGet]
        public ActionResult Reserva(int? ViajeId)
        {
            Session["ApplicationUserId"] = User.Identity.GetUserId();
            Console.Write("ID DEL USUARIO: "+Session["ApplicationUserId"]);

            if (ViajeId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Viaje viaje = db.Viajes.Find(ViajeId);
            if(viaje == null)
            {
                return HttpNotFound();
            }

            else
            {

                ViajeDetalleViewModel detalle = new ViajeDetalleViewModel
                {
                    ViajeId = viaje.ViajeId,
                    ApplicationUserId = viaje.ApplicationUserId,
                    ApplicationUserNombrePiloto = viaje.ApplicationUser.Nombres,
                    ApplicationUserApellidoPiloto = viaje.ApplicationUser.Apellidos,
                    ApplicationUserEdad = viaje.ApplicationUser.Edad,
                    PuntoPartida = viaje.PuntoPartida,
                    PuntoDestino = viaje.PuntoDestino,
                    FechaSalida = viaje.FechaSalida,
                    HoraSalida = viaje.HoraSalida,
                    FechaRegreso = viaje.FechaRegreso,
                    HoraRegreso = viaje.HoraRegreso,
                    Distancia = viaje.Distancia,
                    Duracion = viaje.Duracion,
                    Tarifa = viaje.Tarifa,
                    NumAsientos = viaje.NumAsientos,
                    DetallesViaje = viaje.DetallesViaje,

                    Pasajeros = db.ViajesReservas.Where(x => x.ViajeId == viaje.ViajeId).ToList()
                };
                return View(detalle);
            }            
        }


        //Debe estar logueado para acceder a este metodo
        [Authorize]
        [HttpPost]
        public ActionResult Reserva(string ViajeId, string usuarioViaja, string asientos)
        {
            //Console.WriteLine(" Usuario que viaja: " + usuarioViaja + " Numero de asientos"+ asientos); 
            if (!string.IsNullOrEmpty(ViajeId) && !string.IsNullOrEmpty(usuarioViaja) && !string.IsNullOrEmpty(asientos))
            {
                int idViaje = Convert.ToInt32(ViajeId);
                int numAsientos = Convert.ToInt32(asientos);

                Reserva reserva = new Reserva()
                {
                    ApplicationUserId = usuarioViaja,
                    NumAsientos = numAsientos,
                    Aceptado = true
                };
                db.Reservas.Add(reserva);


                //Descuento de asientos en la tabla Viaje
                var ViajeDescontar = db.Viajes.Find(idViaje);
                if(!(numAsientos > ViajeDescontar.NumAsientos))
                {
                    ViajeDescontar.NumAsientos = ViajeDescontar.NumAsientos - numAsientos;
                    TempData["notificacion"] = "Tu solicitud de reserva ha sido enviada!";
                }
                

                ViajeReserva Viaje_Reserva = new ViajeReserva()
                {
                    ViajeId = idViaje,
                    ReservaId = reserva.ReservaId
                };
                db.ViajesReservas.Add(Viaje_Reserva);

                db.SaveChanges();

                return Redirect("~/Home/Reserva?ViajeId="+idViaje);
                
            }
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        public ActionResult Busqueda()
        {
            return View();
        }


    }
}