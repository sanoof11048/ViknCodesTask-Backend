using System.IdentityModel.Tokens.Jwt;

namespace ViknCodesTask.Middlewares
{
    public class GetUserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GetUserIdMiddleware> _logger;
        public GetUserIdMiddleware(RequestDelegate next, ILogger<GetUserIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken  = handler.ReadJwtToken(token);

                    var userId = jwtToken.Claims.FirstOrDefault(c=>c.Type == "UserId")?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        context.Items["UserId"] = userId;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing JWT token");
                }
            }
            await _next(context);
        }
    }
}
