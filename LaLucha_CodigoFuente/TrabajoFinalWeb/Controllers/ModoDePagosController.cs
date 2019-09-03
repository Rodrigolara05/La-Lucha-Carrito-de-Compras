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
    public class ModoDePagosController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();

        // GET: ModoDePagos
        public ActionResult Index()
        {
            return View(db.ModoDePagoes.ToList());
        }

        // GET: ModoDePagos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModoDePago modoDePago = db.ModoDePagoes.Find(id);
            if (modoDePago == null)
            {
                return HttpNotFound();
            }
            return View(modoDePago);
        }

        // GET: ModoDePagos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModoDePagos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Descripcion")] ModoDePago modoDePago)
        {
            if (ModelState.IsValid)
            {
                db.ModoDePagoes.Add(modoDePago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modoDePago);
        }

        // GET: ModoDePagos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModoDePago modoDePago = db.ModoDePagoes.Find(id);
            if (modoDePago == null)
            {
                return HttpNotFound();
            }
            return View(modoDePago);
        }

        // POST: ModoDePagos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Descripcion")] ModoDePago modoDePago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modoDePago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modoDePago);
        }

        // GET: ModoDePagos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModoDePago modoDePago = db.ModoDePagoes.Find(id);
            if (modoDePago == null)
            {
                return HttpNotFound();
            }
            return View(modoDePago);
        }

        // POST: ModoDePagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModoDePago modoDePago = db.ModoDePagoes.Find(id);
            db.ModoDePagoes.Remove(modoDePago);
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
