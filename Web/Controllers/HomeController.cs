using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            int pageSize = 2;
            int pageNumber = (page ?? 1);

            //PagedList<Viaje> modelPaginado = new PagedList<Viaje>(listaViajes, pageNumber, pageSize);
            
            return View("BusquedaViajes", listaViajes/*.ToPagedList(pageNumber,pageSize)*/);
        }

        [HttpGet]
        public ActionResult Reserva()
        {

            
            //PagedList<Viaje> modelPaginado = new PagedList<Viaje>(listaViajes, pageNumber, pageSize);

            return View();
        }
    }
}