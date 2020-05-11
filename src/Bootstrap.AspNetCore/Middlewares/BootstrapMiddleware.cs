using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

[assembly: InternalsVisibleTo("Gobi.Bootstrap.Tests")]
namespace Gobi.Bootstrap.AspNetCore.Middlewares
{
    internal class BootstrapMiddleware
    {
        private readonly IBootstrapRunner _bootstrapRunner;
        private readonly RequestDelegate _next;

        public BootstrapMiddleware(RequestDelegate next, IBootstrapRunner bootstrapRunner)
        {
            _next = next;
            _bootstrapRunner = bootstrapRunner;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_bootstrapRunner.IsCompleted)
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = 503;
            await context.Response.WriteAsync(_bootstrapRunner.Progress ?? string.Empty);
        }
    }
}