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
    public class PPsController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();

        // GET: PPs
        public ActionResult Index()
        {
            var pPS = db.PPS.Include(p => p.Producto);
            return View(pPS.ToList());
        }

        // GET: PPs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PP pP = db.PPS.Find(id);
            if (pP == null)
            {
                return HttpNotFound();
            }
            return View(pP);
        }

        // GET: PPs/Create
        public ActionResult Create()
        {
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre");
            return View();
        }

        // POST: PPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdProducto,IdPedido,Observacion")] PP pP)
        {
            if (ModelState.IsValid)
            {
                db.PPS.Add(pP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre", pP.IdProducto);
            return View(pP);
        }

        // GET: PPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PP pP = db.PPS.Find(id);
            if (pP == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre", pP.IdProducto);
            return View(pP);
        }

        // POST: PPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IdProducto,IdPedido,Observacion")] PP pP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre", pP.IdProducto);
            return View(pP);
        }

        // GET: PPs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PP pP = db.PPS.Find(id);
            if (pP == null)
            {
                return HttpNotFound();
            }
            return View(pP);
        }

        // POST: PPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PP pP = db.PPS.Find(id);
            db.PPS.Remove(pP);
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
