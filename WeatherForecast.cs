using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ITSoftStore
{
    public class Forbidden : IActionResult
    {
        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
    }
}
