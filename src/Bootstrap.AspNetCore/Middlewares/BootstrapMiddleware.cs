using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gobi.Bootstrap.AspNetCore.Middlewares
{
    internal class BootstrapMiddleware
    {
        private readonly BootstrapRunner _bootstrapRunner;
        private readonly RequestDelegate _next;

        public BootstrapMiddleware(RequestDelegate next, BootstrapRunner bootstrapRunner)
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
            await context.Response.WriteAsync(_bootstrapRunner.Progress);
        }
    }
}