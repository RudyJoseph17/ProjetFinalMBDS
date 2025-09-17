using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BanqueProjet.Web.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log normal via ILogger
                _logger.LogError(ex,
                    "💥 Exception non gérée dans {Path} : {Message}",
                    context.Request.Path,
                    ex.Message);

                // Affiche aussi dans la fenêtre Output de Visual Studio
                Debug.WriteLine($"💥 Exception non gérée : {ex.Message}\nStackTrace: {ex.StackTrace}");

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Une erreur interne est survenue.");
            }
        }
    }
}
