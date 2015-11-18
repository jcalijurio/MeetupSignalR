using System;
using System.Web.Mvc;

namespace DemoWebSocket.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Tela de exemplo de chat via websockets.
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.Title = "WebSockets";

            return View();
        }

        /// <summary>
        /// Tela de exemplo básico de conexão Server Sent Events.
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexEvents()
        {
            ViewBag.Title = "Server Sent Events";

            return View();
        }

        /// <summary>
        /// Tela de exemplo de chat via SignalR.
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexSignal()
        {
            ViewBag.Title = "SignalR";

            return View();
        }

        /// <summary>
        /// Evento invocado pela tela de exemplo Server Sent Events.
        /// </summary>
        /// <returns></returns>
        public ActionResult Evento()
        {
            var data = DateTime.Now.ToString("G");
            Response.CacheControl = "no-cache";
            return Content(string.Format("data: {0}\n\n", "Horário: " + data), "text/event-stream");
        }
    }
}
