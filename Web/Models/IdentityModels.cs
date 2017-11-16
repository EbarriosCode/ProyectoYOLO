using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage ="El {0} es requerido")]
        [Display(Name ="Sexo")]
        public int SexoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaNacimiento { get; set; }

        [NotMapped]
        public int Edad { get { return DateTime.Now.Year - FechaNacimiento.Year; } }

        [Display(Name = "Biografía")]
        [DataType(DataType.MultilineText)]
        public string Biografia { get; set; }

        //Propiedad Virtual para la relacion de Usuario/Sexo
        public virtual Sexo Sexo { get; set; }

        //Propiedad Virtual para la Relación Viaje/Usuario
        public virtual ICollection<Viaje> Viajes { get; set; }

        //Propiedad Virtual para la Relación Viaje/Reserva
        public virtual ICollection<Reserva> Reservas { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUser>().Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<Viaje>().Property(p => p.FechaRegreso).IsOptional();
            modelBuilder.Entity<Viaje>().Property(p => p.HoraRegreso).IsOptional();
            modelBuilder.Entity<Viaje>().Property(p => p.DetallesViaje).IsOptional();
        }

        //public DbSet<ModeloAjax> ModeloAjax { get; set; }
        public DbSet<Viaje> Viajes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<ViajeReserva> ViajesReservas { get; set; }
        public DbSet<TipoVehiculo> TipoVehiculo { get; set; }
        public DbSet<Sexo> Sexo { get; set; }
    }
}