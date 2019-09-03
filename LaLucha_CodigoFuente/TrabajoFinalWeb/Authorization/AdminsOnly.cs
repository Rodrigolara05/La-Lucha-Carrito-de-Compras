
using Restaurante.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoFinalWeb.Models;

namespace Hotel_UPC.Authorization
{

    public class AdminsOnly : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session[SessionName.User] != null)
            {
                Empleado objUser = (Empleado)httpContext.Session["user"];
                if (objUser.TipoEmpleado.Descripcion.ToLower().Equals(UserTypeNames.Admin.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.HttpContext.Response.Redirect("/Home/Index");
            //base.HandleUnauthorizedRequest(filterContext);
        }

    }
}