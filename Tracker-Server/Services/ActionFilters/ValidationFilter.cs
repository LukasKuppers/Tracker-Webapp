using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.ActionFilters
{
    // provides basic input validation, to be used on api actions.
    // tests if input is null or empty

    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var inputs = context.ActionArguments;
            foreach(var param in inputs)
            {
                if (param.Value == null)
                {
                    context.Result = new BadRequestObjectResult("parameter " + param.Key + "is null");
                    return;
                }
                foreach (var property in param.Value.GetType().GetProperties())
                {
                    var value = property.GetValue(param.Value);
                    if (value == null)
                    {
                        context.Result = new BadRequestObjectResult("property " + property.Name + "is null");
                        return;
                    }
                }
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
