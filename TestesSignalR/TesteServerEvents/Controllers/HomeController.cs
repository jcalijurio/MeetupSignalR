using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace TesteServerEvents.Controllers
{
    public class HomeController : Controller
    {
        private static BlockingCollection<string> _data = new BlockingCollection<string>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Evento()
        {
            var data = DateTime.Now.ToString("G");
            Response.CacheControl = "no-cache";
            return Content(string.Format("data: {0}\n\n", "Horário: " + data), "text/event-stream");
        }
    }
}