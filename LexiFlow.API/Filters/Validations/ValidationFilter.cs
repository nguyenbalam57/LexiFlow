using LexiFlow.API.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace LexiFlow.API.Filters
{
    /// <summary>
    /// Filter để xử lý lỗi validation
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        /// <summary>
        /// Thực thi trước khi action được gọi
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var errorResponse = new ErrorResponse
                {
                    Message = "Validation failed",
                    ErrorCode = "VALIDATION_ERROR",
                    Details = errors
                };

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        /// <summary>
        /// Thực thi sau khi action được gọi
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed
        }
    }
}