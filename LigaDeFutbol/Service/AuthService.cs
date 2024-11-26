using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

public class AuthService : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Verificar si el usuario está autenticado
        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
          
            var header = context.HttpContext.Request.Headers.Authorization.ToString();
            var token = header.Substring("Bearer ".Length);
            var resultado=new JsonResult(new 
            { error = "Acceso no autorizado" 
            
            })
            {
               StatusCode =StatusCodes.Status401Unauthorized
            };

            //asigna el resultado y cancela la ejecución del controlador
            context.Result = resultado;
            return;
        }
        await next();
    }
}
