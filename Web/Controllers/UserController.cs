using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize]
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            // LISTAR USUARIOS DE ASPNETUSERS
            /*var ctx = Request.GetOwinContext().Get<ApplicationDbContext>();
            var users = ctx.Users;

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(ctx));
            var users2 = manager.Users.ToList(); */

            return View();
        }

        public ActionResult Perfil()
        {
            // Contexto para recuperar el Usuario Logueado Actualmente
            var ctx = Request.GetOwinContext().Get<ApplicationDbContext>();
            string IdUser = User.Identity.GetUserId();

            ApplicationUser Logueado = ctx.Users.FirstOrDefault(x => x.Id == IdUser);
            return View(Logueado);
        }

        [HttpGet]
        public ActionResult MisViajes(int? page)
        {
            string IdUser = User.Identity.GetUserId();

            List<Viaje> misViajes = db.Viajes.
                                    Where(x => x.ApplicationUserId == IdUser).
                                    OrderByDescending(x => x.ViajeId).
                                    ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(misViajes.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Viajes()
        {
            Session["ApplicationUserId"] = User.Identity.GetUserId();
            //ViewBag.UserId = Session["ApplicationUserId"].ToString();

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Viajes([Bind(Include = "Fecha,ApplicationUserId,PuntoPartida,PuntoDestino,FechaSalida,HoraSalida,FechaRegreso,HoraRegreso")]Viaje model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        model.Fecha = DateTime.Now;
        //        //model.ApplicationUserId = User.Identity.GetUserId();                
        //        db.Viajes.Add(model);
        //        db.SaveChanges();
        //        return RedirectToAction("Perfil");
        //    }

        //    return View(model);            
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Viajes(Viaje model)
        {

            if (ModelState.IsValid)
            {
                ViajeViewModel ViewModel = new ViajeViewModel();
                ViewModel.Fecha = DateTime.Now;
                ViewModel.PuntoPartida = model.PuntoPartida;
                ViewModel.PuntoDestino = model.PuntoDestino;
                ViewModel.ApplicationUserId = model.ApplicationUserId;
                ViewModel.FechaSalida = model.FechaSalida;
                ViewModel.HoraSalida = model.HoraSalida;
                ViewModel.FechaRegreso = model.FechaRegreso;
                ViewModel.HoraRegreso = model.HoraRegreso;
                ViewModel.Distancia = model.Distancia;
                ViewModel.Duracion = model.Duracion;

                ViewBag.TipoVehiculoList = new SelectList(db.TipoVehiculo.ToList(), "TipoVehiculoId", "NombreTipoVehiculo");
                return View("ViajesContinuacion", ViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult InsertViaje(ViajeViewModel ViewModel)
        {
            ViewModel.ToString();

            Viaje viaje = new Viaje();

            ViewModel.Fecha = DateTime.Now;
            ViewModel.ToString();
            if (ModelState.IsValid)
            {
                viaje.Fecha = ViewModel.Fecha;
                viaje.PuntoPartida = ViewModel.PuntoPartida;
                viaje.PuntoDestino = ViewModel.PuntoDestino;
                viaje.ApplicationUserId = ViewModel.ApplicationUserId;
                viaje.FechaSalida = ViewModel.FechaSalida;
                viaje.HoraSalida = ViewModel.HoraSalida;
                viaje.FechaRegreso = ViewModel.FechaRegreso;
                viaje.HoraRegreso = ViewModel.HoraRegreso;
                viaje.Distancia = ViewModel.Distancia;
                viaje.Duracion = ViewModel.Duracion;
                viaje.Tarifa = ViewModel.Tarifa;
                viaje.NumAsientos = ViewModel.NumAsientos;
                viaje.DetallesViaje = ViewModel.DetallesViaje;
                viaje.TipoVehiculoId = ViewModel.TipoVehiculoId;

                db.Viajes.Add(viaje);
                db.SaveChanges();
                return RedirectToAction("MisViajes");
                //return Json(ViewModel, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Perfil");
            }
            return View("ViajesContinuacion", ViewModel);
            //return View("ViajesContinuacion", ViewModel);
        }

        [HttpGet]
        public ActionResult MisReservas(int? page)
        {
            string IdUser = User.Identity.GetUserId();
            
            //List<Reserva> misReservas = db.Reservas.                                    
            //                        Where(x => x.ApplicationUserId == IdUser).
            //                        OrderByDescending(x => x.ReservaId).                                    
            //                        ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);


            //Reservas con ViewModel SE ENVIA A LA VISTA
            var MisReservasList = db.ViajesReservas.Where(x => x.Reserva.ApplicationUserId == IdUser).ToList();
            List<MisReservasViewModel> rsViewModel = new List<MisReservasViewModel>();

            foreach (var rs in MisReservasList)
            {
                MisReservasViewModel obj = new MisReservasViewModel();

                obj.NombreConductor = rs.Viaje.ApplicationUser.Nombres;
                obj.ApellidoConductor = rs.Viaje.ApplicationUser.Apellidos;
                obj.TelefonoConductor = rs.Viaje.ApplicationUser.PhoneNumber;
                obj.PuntoSalida = rs.Viaje.PuntoPartida;
                obj.PuntoDestino = rs.Viaje.PuntoDestino;
                obj.FechaSalida = rs.Viaje.FechaSalida;
                obj.HoraSalida = rs.Viaje.HoraSalida;
                obj.FechaRegreso = rs.Viaje.FechaRegreso;
                obj.HoraRegreso = rs.Viaje.HoraRegreso;
                obj.Distancia = rs.Viaje.Distancia;
                obj.Duracion = rs.Viaje.Duracion;
                obj.Tarifa = rs.Viaje.Tarifa;
                obj.NumAsientosReservados = rs.Reserva.NumAsientos;

                rsViewModel.Add(obj);
            }

            return View(rsViewModel.ToPagedList(pageNumber, pageSize));           
        }
       

        public ActionResult ModeloAjax()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModeloAjax(ModeloAjax model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //db.ModeloAjax.Add(model);
            db.SaveChanges();
            //return RedirectToAction("Viajes");            

            model = null;
            return View(model);
        }

        public ActionResult ReservasViajeDetalle(int idViaje)
        {
            Console.Write(idViaje);

            var listaReservaciones = db.ViajesReservas.Where(x => x.ViajeId == idViaje).ToList();
            return PartialView("_ReservasViajeDetalle", listaReservaciones);
        }

        public ActionResult Mapa()
        {
            return View();
        }

        public ActionResult Configuracion()
        {
            return View();
        }

        public ActionResult Politicas()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}