using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrabajoFinalWeb.Models;
using System.Windows.Forms;
using Hotel_UPC.Authorization;
using Restaurante.Constantes;

namespace TrabajoFinalWeb.Controllers
{
    [UserLoggedOn]
    [AdminsOnly]
    public class DetalleBoletasController : Controller
    {
        private RestauranteEntitiesContext db = new RestauranteEntitiesContext();
        // GET: DetalleBoletas
        public ActionResult Index()
        {
            var detalleBoletas = db.DetalleBoletas.Include(d => d.ModoDePago).Include(d => d.Pedido);
            return View(detalleBoletas.ToList());
        }

        [AllowAnonymous]
        // GET: DetalleBoletas
        public ActionResult Index_Especifico()
        {
            int id_Pedido = (from c in db.Pedidoes select c).Count();
            var detalleBoletas = from c in db.DetalleBoletas where c.IdPedido == id_Pedido select c;
            return View(detalleBoletas.ToList());
        }
        
        public ActionResult generarCodigoQR(int? id)
        {
            //DetalleBoleta detalle = db.DetalleBoletas.Find(id);
            //var detalleBoletas = db.DetalleBoletas.Include(d => d.ModoDePago).Include(d => d.Pedido);
            //String monto = Convert.ToString(detalle.MontoTotal);
            //QRCodeEncoder encoder = new QRCodeEncoder();
            //Bitmap img = encoder.Encode(monto);
            //Image QR = (Image)img;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    QR.Save(ms, ImageFormat.Jpeg);
            //    byte[] imageBytes = ms.ToArray();
            //    ViewBag.Texto = "data:image/gif;base64" + Convert.ToBase64String(imageBytes);
            //}
            return RedirectToAction("Index_Especifico");
        }
        // GET: DetalleBoletas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleBoleta detalleBoleta = db.DetalleBoletas.Find(id);
            if (detalleBoleta == null)
            {
                return HttpNotFound();
            }
            return View(detalleBoleta);
        }

        // GET: DetalleBoletas/Create
        public ActionResult Create()
        {
            ViewBag.IdModoDePago = new SelectList(db.ModoDePagoes, "ID", "Descripcion");
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle");
            return View();
        }

        // POST: DetalleBoletas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdPedido,MontoTotal,IdModoDePago")] DetalleBoleta detalleBoleta)
        {
            if (ModelState.IsValid)
            {
                db.DetalleBoletas.Add(detalleBoleta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdModoDePago = new SelectList(db.ModoDePagoes, "ID", "Descripcion", detalleBoleta.IdModoDePago);
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle", detalleBoleta.IdPedido);
            return View(detalleBoleta);
        }

        // GET: DetalleBoletas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleBoleta detalleBoleta = db.DetalleBoletas.Find(id);
            if (detalleBoleta == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdModoDePago = new SelectList(db.ModoDePagoes, "ID", "Descripcion", detalleBoleta.IdModoDePago);
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle", detalleBoleta.IdPedido);
            return View(detalleBoleta);
        }

        // POST: DetalleBoletas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IdPedido,MontoTotal,IdModoDePago")] DetalleBoleta detalleBoleta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleBoleta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdModoDePago = new SelectList(db.ModoDePagoes, "ID", "Descripcion", detalleBoleta.IdModoDePago);
            ViewBag.IdPedido = new SelectList(db.Pedidoes, "ID", "Detalle", detalleBoleta.IdPedido);
            return View(detalleBoleta);
        }

        // GET: DetalleBoletas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleBoleta detalleBoleta = db.DetalleBoletas.Find(id);
            if (detalleBoleta == null)
            {
                return HttpNotFound();
            }
            return View(detalleBoleta);
        }

        // POST: DetalleBoletas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleBoleta detalleBoleta = db.DetalleBoletas.Find(id);
            db.DetalleBoletas.Remove(detalleBoleta);
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
        //Pedido_Boleta

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pedido_Boleta()
        {
            int indice = (from c in db.PS select c).Count();
            ViewBag.MontoTotal = (from a in db.PPS.
                            Include("Producto").Include("P").AsEnumerable()
                                  where a.IdPedido == indice
                                  select a.Producto.Precio).Sum();
            ViewBag.IdModoDePago = new SelectList(db.ModoDePagoes, "ID", "Descripcion");
            return View();
        }
        //RegistrarPedido

        [AllowAnonymous]
        public IEnumerable<TrabajoFinalWeb.Models.P> Getpedidos()
        {
            return db.PS.AsEnumerable<TrabajoFinalWeb.Models.P>();
        }

        [AllowAnonymous]
        public IEnumerable<TrabajoFinalWeb.Models.PP> Getproducto_pedidos()
        {
            return db.PPS.AsEnumerable<TrabajoFinalWeb.Models.PP>();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarPedido(int IdModoDePago)
        {
            IEnumerable<TrabajoFinalWeb.Models.P> lista = Getpedidos();
            IEnumerable<TrabajoFinalWeb.Models.PP> listaPP = Getproducto_pedidos();

            Pedido pedido = new Pedido();
            Productos_Pedidos producto_pedido = new Productos_Pedidos();
            List<Productos_Pedidos> arreglo = new List<Productos_Pedidos>();
            Empleado objUser = (Empleado)Session[SessionName.User];
            for (int i = 0; i < db.PS.Count(); i++)
            {
                //pedido.ID = (int)lista.ElementAt(i).ID;
                pedido.Atendido = (bool)lista.ElementAt(i).Atendido;
                pedido.Detalle = objUser.Nombre;
                pedido.IdEmpleado = "ADMINMax";
            }
            db.Pedidoes.Add(pedido);
            db.SaveChanges();
            int id_Pedido= (from c in db.Pedidoes select c).Count();
            for (int i = 0; i < db.PPS.Count(); i++)
            {
                producto_pedido.IdPedido = id_Pedido;
                producto_pedido.IdProducto = (int)listaPP.ElementAt(i).IdProducto;
                db.Productos_Pedidos.Add(producto_pedido);
                db.SaveChanges();
            }
            DetalleBoleta detalle = new DetalleBoleta();
            detalle.IdModoDePago = IdModoDePago;
            detalle.IdPedido = id_Pedido;
            decimal montoTotal= (from a in db.Productos_Pedidos.
                              Include("Producto").Include("Pedido").AsEnumerable()
                             where a.IdPedido == id_Pedido
                             select a.Producto.Precio).Sum();
            detalle.MontoTotal = montoTotal;
            db.DetalleBoletas.Add(detalle);
            db.SaveChanges();
            db.eliminarTodo();
            db.SaveChanges();
            var detalleBoletas = db.DetalleBoletas.Include(d => d.ModoDePago).Include(d => d.Pedido);
            return RedirectToAction("Index_Especifico");
        }
    }
}
