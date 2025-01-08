using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SmartCredit.FrontEnd.WebApp.Filters
{
    public class CustomAuthorize : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            var excludedRoutes = new HashSet<(string Controller, string Action)>
            {
                ("home", "index"),
                ("creditcards", "index")
            };
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            if (excludedRoutes.Contains((controllerName.ToLower(), actionName.ToLower())))
            {
                await next();
                return;
            }


            // Verificar si la cookie existe
            if (!request.Cookies.ContainsKey("CardIdSelected"))
            {
                context.Result = new RedirectToActionResult("Index", "CreditCards", null);
                return;
            }

            // Continuar con la ejecución de la acción
            await next();
        }
    }
}
