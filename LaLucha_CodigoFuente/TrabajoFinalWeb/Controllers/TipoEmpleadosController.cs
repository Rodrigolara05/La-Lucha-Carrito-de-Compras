using Hotel_UPC.Authorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrabajoFinalWeb.Models;

namespace TrabajoFinalWeb.Controllers
{
    [UserLoggedOn]
    [AdminsOnly]
    public class TipoEmpleadosController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();

        // GET: TipoEmpleados
        public ActionResult Index()
        {
            return View(db.TipoEmpleadoes.ToList());
        }

        // GET: TipoEmpleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEmpleado tipoEmpleado = db.TipoEmpleadoes.Find(id);
            if (tipoEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(tipoEmpleado);
        }

        // GET: TipoEmpleados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoEmpleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Descripcion,Sueldo")] TipoEmpleado tipoEmpleado)
        {
            if (ModelState.IsValid)
            {
                db.TipoEmpleadoes.Add(tipoEmpleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoEmpleado);
        }

        // GET: TipoEmpleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEmpleado tipoEmpleado = db.TipoEmpleadoes.Find(id);
            if (tipoEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(tipoEmpleado);
        }

        // POST: TipoEmpleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Descripcion,Sueldo")] TipoEmpleado tipoEmpleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEmpleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoEmpleado);
        }

        // GET: TipoEmpleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEmpleado tipoEmpleado = db.TipoEmpleadoes.Find(id);
            if (tipoEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(tipoEmpleado);
        }

        // POST: TipoEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoEmpleado tipoEmpleado = db.TipoEmpleadoes.Find(id);
            db.TipoEmpleadoes.Remove(tipoEmpleado);
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
    }
}
