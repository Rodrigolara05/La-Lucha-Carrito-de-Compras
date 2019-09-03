using Hotel_UPC.Authorization;
using Restaurante.Constantes;
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
    public class Productos_PedidosController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();

        private void creacionPedido() //CrearNuevoPedido
        {
            //CrearNuevoPedido
            P objNuevoPedido = new P();
            objNuevoPedido.Atendido = false;
            db.PS.Add(objNuevoPedido);
            db.SaveChanges();
            //Pedido objNuevoPedido = new Pedido();
            //objNuevoPedido.Atendido = false;
            //objNuevoPedido.Detalle = txtNombreCliente.Text;
            //objNuevoPedido.IdEmpleado = "ADMINMax";
        }

        // GET: Productos_Pedidos
        public ActionResult Index()
        {
            var productos_Pedidos = db.Productos_Pedidos.Include(p => p.Pedido).Include(p => p.Producto);
            return View(productos_Pedidos.ToList());
        }

        // GET: Productos_Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos_Pedidos productos_Pedidos = db.Productos_Pedidos.Find(id);
            if (productos_Pedidos == null)
            {
                return HttpNotFound();
            }
            return View(productos_Pedidos);
        }

        // GET: Productos_Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle");
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre");
            return View();
        }

        // POST: Productos_Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdProducto,IdPedido,Observacion")] Productos_Pedidos productos_Pedidos)
        {
            if (ModelState.IsValid)
            {
                db.Productos_Pedidos.Add(productos_Pedidos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle", productos_Pedidos.IdPedido);
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre", productos_Pedidos.IdProducto);
            return View(productos_Pedidos);
        }

        // GET: Productos_Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos_Pedidos productos_Pedidos = db.Productos_Pedidos.Find(id);
            if (productos_Pedidos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle", productos_Pedidos.IdPedido);
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre", productos_Pedidos.IdProducto);
            return View(productos_Pedidos);
        }

        // POST: Productos_Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IdProducto,IdPedido,Observacion")] Productos_Pedidos productos_Pedidos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productos_Pedidos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle", productos_Pedidos.IdPedido);
            ViewBag.IdProducto = new SelectList(db.Productoes, "ID", "Nombre", productos_Pedidos.IdProducto);
            return View(productos_Pedidos);
        }

        // GET: Productos_Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos_Pedidos productos_Pedidos = db.Productos_Pedidos.Find(id);
            if (productos_Pedidos == null)
            {
                return HttpNotFound();
            }
            return View(productos_Pedidos);
        }

        // POST: Productos_Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos_Pedidos productos_Pedidos = db.Productos_Pedidos.Find(id);
            db.Productos_Pedidos.Remove(productos_Pedidos);
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

        // GET: Productos_Pedidos/Create
        public ActionResult Orden()
        {
            int cont = 0;
            var lista_de_productos = from c in db.Productoes
                        select c;
            ViewBag.Productos = lista_de_productos.ToList();
            ViewBag.Contador = cont;
            Empleado objUser = (Empleado)Session[SessionName.User];
            ViewBag.Nombre =  objUser.Nombre;
            return View(new List<PP>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orden(int id,int ncontador)
        {
            //Producto producto = db.Productoes.Find(id);
            if (ncontador == 0)
            {
                creacionPedido();
                ncontador++;
            }
            PP aux = new PP();
            aux.IdProducto = id;
            aux.IdPedido = (from c in db.PS select c).Count();
            if (ModelState.IsValid)
            {
                db.PPS.Add(aux);
                db.SaveChanges();
            }
            //Productos
            var lista_de_productos = from c in db.Productoes
                                     select c;
            ViewBag.Productos = lista_de_productos.ToList();
            ViewBag.Contador = ncontador;

            return View(db.PPS.ToList());
        }
    }
}
