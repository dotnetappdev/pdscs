using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UKParliament.CodeTest.Web.Middleware
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors?.Select(e => e.ErrorMessage).ToArray() ?? new string[0]
                    );

                // If errors only contain keys like "person" or "$", it's a binding error, not FluentValidation
                var onlyBindingErrors = errors.Keys.All(k => k == "person" || k.StartsWith("$"));
                if (onlyBindingErrors)
                {
                    context.Result = new BadRequestObjectResult(new { message = "Model binding failed", errors });
                }
                else
                {
                    // Otherwise, return field-level errors (FluentValidation)
                    context.Result = new BadRequestObjectResult(new { message = "Validation failed", errors });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}