namespace SistemaGestionPeluqueria.ApiWeb.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        public readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Ocurrio un error intesperado"+ ex.Message);
            }
        }
    }
}
