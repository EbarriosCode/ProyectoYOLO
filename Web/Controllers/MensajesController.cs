using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Web.Hubs;
using Web.Models;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class MensajesController : Controller
    {
        // GET: Mensajes
        public ActionResult Index()
        {
            if(Session["IdUsuarioConductor"] != null)
            {
                Console.Write("Si hay conductor");
                ViewBag.UserDestino = Session["IdUsuarioConductor"];
                ViewBag.UsuarioEnvia = User.Identity.GetUserId();
            }
            else
            {
                Console.Write("No hay conductor");
            }
            
            return View();
        }

        public ActionResult EnviarMensaje(string idConductor)
        {
            //Este id se envia al index para enviar el mensaje se almacena en una variable de session
            Session["IdUsuarioConductor"] = idConductor;
            return RedirectToAction("Index");
        }

        public ActionResult Contactos(string usuario)
        {
            //Listar Usuarios de AspNetUsers
            var ctx = Request.GetOwinContext().Get<ApplicationDbContext>();
            var users = ctx.Users;

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(ctx));
            var users2 = (from u in manager.Users
                         where (u.Nombres.Contains(usuario)
                         ||
                         u.Apellidos.Contains(usuario)
                         ||
                         u.UserName.Contains(usuario))
                         select u).ToList();

            if (string.IsNullOrEmpty(usuario))
            {
                users2 = manager.Users.ToList();
            }
             

            return View(users2);
        }

        [HttpPost]
        public ActionResult InsertMensaje(Mensajes model)
        {
            model.ToString();
            model.FechaCreacion = DateTime.Now;

            if(ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Mensajes.Add(model);

                    Conversacion conversacion = new Conversacion
                    {
                        ApplicationUserId = model.UsuarioRecibe,
                        MensajeId = model.MensajeId
                    };

                    context.Conversaciones.Add(conversacion);
                    context.SaveChanges();
                }
            }

            ChatHub.NotifyCurrentEmployeeInformationToAllClients();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetAllMessages()
        {
            string sesionLogueado = User.Identity.GetUserId().ToString();
            string sesionConductor = Session["IdUsuarioConductor"].ToString();

            List<Mensajes> ConversacionesList = new List<Mensajes>();

            using (var context = new ApplicationDbContext())
            {
                ConversacionesList = (from m in context.Mensajes
                                      where (m.UsuarioEnvia == sesionLogueado
                                      &&
                                      (m.UsuarioRecibe == sesionConductor))
                                      ||
                                      ((m.UsuarioRecibe == sesionLogueado)
                                      &&
                                      (m.UsuarioEnvia == sesionConductor))
                                      select m).ToList();

                List<ConversacionViewModel> lista = new List<ConversacionViewModel>();

                foreach(var msg in ConversacionesList)
                {
                    ConversacionViewModel conv = new ConversacionViewModel();

                    conv.NombreUserEnvia = msg.ApplicationUserEnvia.UserName;
                    conv.NombreUserRecibe = msg.ApplicationUserRecibe.UserName;
                    conv.ListaMensajes = msg.Mensaje;
                    conv.FechaEnvio = msg.FechaCreacion;

                    lista.Add(conv);
                }
                return PartialView("_MensajesPartial", lista);
            }            

        }


        //ActionResult para chat de Pasajeros
        [HttpGet]
        public ActionResult ChatDefault()
        {
            if (Session["IdUsuarioConductor"] != null)
            {
                Console.Write("Si hay conductor");
                ViewBag.UserDestino = Session["IdUsuarioConductor"];
                ViewBag.UsuarioEnvia = User.Identity.GetUserId();
            }
            else
            {
                Console.Write("No hay conductor");
            }

            return View();
        }
        //ActionResult para chat de Pasajeros
        [HttpPost]
        public ActionResult ChatDefault(string IdUsuarioConductor)
        {
            Session["IdUsuarioConductor"] = IdUsuarioConductor;

            return RedirectToAction("ChatDefault");
        }
    }

}