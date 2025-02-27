using Bussiness;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPracticaRFC.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Index()
        {
            return View("Inicio");
        }
        public ActionResult VistaIngresar()
        {
            return View("IngresarDatos");
        }
        public ActionResult AgregarUsuario(E_Usuario usuario)
        {
            try
            {
                B_Usuario Bussiness = new B_Usuario();
                Bussiness.AgregarUsuario(usuario);
                return View("RFCGenerado", usuario);
            }
            catch (Exception ex)
            {
                TempData["ERROR"] = $"{ex.Message}";
                return View("IngresarDatos");
            }
        }
        public ActionResult VistaBaseDatos()
        {
            List<E_Usuario> lista = new List<E_Usuario>();
            B_Usuario bussiness = new B_Usuario();
            lista = bussiness.Obtenertodos();
            return View("BaseDatos", lista);
        }
        public ActionResult VistaEditar(int ID)
        {
            try
            {
                B_Usuario bussiness = new B_Usuario();
                E_Usuario usuario = new E_Usuario();
                usuario = bussiness.ObtenerUsuario(ID);
                return View("EditarUsuario", usuario);
            }
            catch(Exception ex)
            {
                TempData["ERROR"] = $"{ex.Message}";
                return RedirectToAction("VistaBaseDatos");
            }
        }
        public ActionResult EditarUsuario(E_Usuario usuario)
        {
            try
            {
                B_Usuario bussiness = new B_Usuario();
                bussiness.ActualizarUsuario(usuario);
                TempData["mensaje"] = $"{usuario.nombre} {usuario.apellidoPaterno} {usuario.apellidoMaterno} actualizado";

                return RedirectToAction("VistaBaseDatos");
            }
            catch(Exception ex)
            {
                TempData["ERROR"] = $"{ex.Message}";
                return RedirectToAction("EditarUsuario");
            }
        }
        public ActionResult EliminarUsuario(int ID)
        {
            try
            {
                B_Usuario bussiness = new B_Usuario();
                E_Usuario usuario = bussiness.ObtenerUsuario(ID);
                bussiness.BorrarUsuario(ID);
                TempData["mensaje"] = $"{usuario.nombre} {usuario.apellidoPaterno} {usuario.apellidoMaterno} eliminado";
            }
            catch (Exception ex)
            {
                TempData["ERROR"] = $"{ex.Message}";
            }
            return RedirectToAction("VistaBaseDatos");
        }
        public ActionResult Buscar(string Buscador)
        {
            List<E_Usuario> lista = new List<E_Usuario>();
            B_Usuario bussiness = new B_Usuario();
            lista = bussiness.Buscador(Buscador);
            return View("BaseDatos", lista);
        }

    }
}