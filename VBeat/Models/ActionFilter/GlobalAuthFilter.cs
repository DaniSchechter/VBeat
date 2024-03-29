﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using VBeat.Models.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace VBeat.Models.ActionFilter
{
    public class GlobalAuthFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

            string path = context.HttpContext.Request.Path;
            if (path == "/" || path == "/ArtistModels/Create" || path == "/UserModels/Create" || path == "/UserModels/SignIn")
            {
                return;
            }

            if (!context.HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                context.Result = new RedirectToRouteResult(
                       new RouteValueDictionary(new { controller = "UserModels", action = "SignIn" })
                );

                

                context.Result.ExecuteResultAsync(((Controller)context.Controller).ControllerContext);
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
