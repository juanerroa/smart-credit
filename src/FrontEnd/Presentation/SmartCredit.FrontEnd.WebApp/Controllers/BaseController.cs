using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCredit.FrontEnd.WebApp.Helpers;

namespace SmartCredit.FrontEnd.WebApp.Controllers
{
    public class BaseController : Controller
    {

    
        //Método que se ejecuta en cada vista para que todas tengan el CardId seleccionado.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["CardIdSelected"] = GetCardIdSelected();
            base.OnActionExecuting(context);
        }


        //Metodo a heredar para que todos los controladores sepan cual es el el CardId seleccionado.
        protected string GetCardIdSelected()
        {
            if (Request.Cookies.TryGetValue("CardIdSelected", out var cardIdSelected))
            {
                return cardIdSelected;
            }
            return null; // O un valor por defecto si no existe la cookie
        }

    }
}
