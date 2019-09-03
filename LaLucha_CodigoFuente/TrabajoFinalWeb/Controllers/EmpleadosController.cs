using Hotel_UPC.Authorization;
using Restaurante.Constantes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TrabajoFinalWeb.Models;

namespace TrabajoFinalWeb.Controllers
{
    [UserLoggedOn]
    [AdminsOnly]
    public class EmpleadosController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();

        // GET: Empleados
        public ActionResult Index()
        {
            var empleadoes = db.Empleadoes.Include(e => e.TipoEmpleado);
            return View(empleadoes.ToList());
        }

        // GET: Empleados/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }
        //Registrar_Personal
        public ActionResult Registrar_Personal()
        {
            ViewBag.IdTipoEmpleado = new SelectList(db.TipoEmpleadoes, "ID", "Descripcion");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar_Personal([Bind(Include = "ID,Contraseña,IdTipoEmpleado,Nombre,Celular,Correo,Direccion,Sueldo")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.Sueldo = 0;
                db.Empleadoes.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTipoEmpleado = new SelectList(db.TipoEmpleadoes, "ID", "Descripcion", empleado.IdTipoEmpleado);
            return View(empleado);
        }
        [AllowAnonymous]
        // GET: Empleados/Create
        public ActionResult Create()
        {
            ViewBag.IdTipoEmpleado = new SelectList(db.TipoEmpleadoes, "ID", "Descripcion");
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Contraseña,IdTipoEmpleado,Nombre,Celular,Correo,Direccion,Sueldo")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.Sueldo = 0;
                empleado.IdTipoEmpleado = 5;
                db.Empleadoes.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IdTipoEmpleado = new SelectList(db.TipoEmpleadoes, "ID", "Descripcion", empleado.IdTipoEmpleado);
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTipoEmpleado = new SelectList(db.TipoEmpleadoes, "ID", "Descripcion", empleado.IdTipoEmpleado);
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Contraseña,IdTipoEmpleado,Nombre,Celular,Correo,Direccion,Sueldo")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTipoEmpleado = new SelectList(db.TipoEmpleadoes, "ID", "Descripcion", empleado.IdTipoEmpleado);
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleadoes.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Empleado empleado = db.Empleadoes.Find(id);
            db.Empleadoes.Remove(empleado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "ID,Contraseña,IdTipoEmpleado,Nombre,Celular,Correo,Direccion,Sueldo")] Empleado user)
        {
            if (ModelState.IsValid)
            {
                var us = from u in db.Empleadoes.Include("TipoEmpleado")
                         where u.ID == user.ID && u.Contraseña == user.Contraseña
                         select u;
                Empleado user2 = us.FirstOrDefault();
                if (user2 != null)
                {
                    Session[SessionName.User] = user2;
                    if (user2.IdTipoEmpleado == 1)
                        return RedirectToAction("Index_Administrador", "Home");
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "invalido ctv >:v";
                }


            }
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        //Recuperar
        [AllowAnonymous]
        // GET: Empleados/Create
        public ActionResult Recuperar()
        {
            return View();
        }
        [AllowAnonymous]
        public void enviarEMail(string correo,string password)
        {
            MailMessage Correo = new MailMessage();
            Correo.From = new MailAddress("Correodelalucha@gmail.com");
            Correo.To.Add(correo);
            Correo.Subject=("Recuperar Contraseña La Lucha");
            Correo.Body = "Hola! Buen dia, Usted solicito recuperar su contraseña: " + password;
            Correo.Priority = MailPriority.Normal;

            SmtpClient ServerEmail = new SmtpClient();
            ServerEmail.Credentials = new NetworkCredential("Correodelalucha@gmail.com", "contraseñadelalucha");
            ServerEmail.Host = "smtp.gmail.com";
            ServerEmail.Port = 587;
            ServerEmail.EnableSsl = true;
            try
            {
                ServerEmail.Send(Correo);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            Correo.Dispose();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recuperar([Bind(Include = "ID,Contraseña,IdTipoEmpleado,Nombre,Celular,Correo,Direccion,Sueldo")] Empleado empleado)
        {
            var us = from u in db.Empleadoes.Include("TipoEmpleado")
                     where u.Correo == empleado.Correo
                     select u;
            Empleado usuario = us.FirstOrDefault();
            enviarEMail(usuario.Correo, usuario.Contraseña);
            return RedirectToAction("Index", "Home");
        }

    }
}
