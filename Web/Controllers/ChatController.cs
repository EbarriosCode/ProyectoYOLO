using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Web.Hubs;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        List<Mensajes> ChatsList = new List<Mensajes>();
        
        // GET: Chat
        public ActionResult Index()
        {
            
            using (var context = new ApplicationDbContext())
            {
                ChatsList = context.Mensajes
                                   .Include("ApplicationUserEnvia")
                                   .Include("ApplicationUserRecibe")
                                   .ToList();
            }
            
            ViewBag.UsuarioEnvia = User.Identity.GetUserId();
            ViewBag.NombreUsuarioEnvia = User.Identity.GetUserName();
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string IdUsuarioConductor)
        //{
        //    Session["IdUsuarioConductor"] = IdUsuarioConductor;
            
        //    return RedirectToAction("Index");
        //}


        //ActionResult para chat de Pasajeros
        [HttpGet]
        public ActionResult ChatDefault()
        {

            

            return View();
        }

        //ActionResult para chat de Pasajeros
        [HttpPost]
        public ActionResult ChatDefault(string IdUsuarioConductor)
        {
            Session["IdUsuarioConductor"] = IdUsuarioConductor;

            return RedirectToAction("ChatDefault");
        }

        [HttpGet]
        public ActionResult GetAllMessagesAdmin()
        {
            using (var context = new ApplicationDbContext())
            {
                ChatsList = context.Mensajes.ToList();                                
            }
            return PartialView("_ChatMessages", ChatsList);
        }

        [HttpGet]
        public ActionResult GetAllMessages()
        {
            using (var context = new ApplicationDbContext())
            {
                ChatsList = context.Mensajes.ToList();

                var ctx = Request.GetOwinContext().Get<ApplicationDbContext>();
                var user = ctx.Users.Find(User.Identity.GetUserId());
                var userConductor = ctx.Users.Find(Session["IdUsuarioConductor"].ToString());

                ViewBag.NombreUserPasajero = user.Nombres;
                ViewBag.NombreUserConductor = userConductor.Nombres;
                //Chat mensaje = new Chat();
                //mensaje = context.Mensajes.Where(c => c.UsuarioEnvia = Session["IdUsuarioConductor"].ToString());
                //var message = from mensaje in context.Mensajes
                //              where mensaje.UsuarioEnvia == User.Identity.GetUserId().ToString()
                //              &&
                //                    mensaje.UsuarioRecibe == Session["IdUsuarioConductor"].ToString()
                //              select mensaje;
                //ViewBag.IdUserPasajero = User.Identity.GetUserId();
                //ViewBag.NombreUserPasajero = User.Identity.GetUserName();
            }
            return PartialView("_ChatMessages", ChatsList);
        }

        //Insert Message Record
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Mensajes model)
        {
            //model.ToString();
            model.FechaCreacion = DateTime.Now;
            if (ModelState.IsValid)
            {
                //Insert into Message table                 
                using (var context = new ApplicationDbContext())
                {
                    context.Mensajes.Add(model);
                    context.SaveChanges();
                }
            }

            //Once the record is inserted , then notify all the subscribers (Clients)
            ChatHub.NotifyCurrentEmployeeInformationToAllClients();
            return RedirectToAction("Index");
        }

        //Insert Message Record
        public ActionResult InsertDefault()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertDefault(Mensajes model)
        {
            //model.ToString();
            model.FechaCreacion = DateTime.Now;
            if (ModelState.IsValid)
            {
                //Insert into Message table                 
                using (var context = new ApplicationDbContext())
                {
                    context.Mensajes.Add(model);
                    context.SaveChanges();
                }
            }

            //Once the record is inserted , then notify all the subscribers (Clients)
            ChatHub.NotifyCurrentEmployeeInformationToAllClients();
            return RedirectToAction("ChatDefault");
        }
    }
}