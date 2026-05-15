namespace MedSchedulerUZ.API.Middlewares
{
    public class MustChangePasswordMiddleware
    {
        private readonly RequestDelegate _next;

        public MustChangePasswordMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var mustChangePassword = context.User.FindFirst("must_change_password")?.Value;

            if (mustChangePassword == "True")
            {
                var path = context.Request.Path.Value?.ToLower();

                var allowedPaths = new[]
                {
                    "/api/user/change-password",
                    "/api/user/login",
                    "/api/user/logout"
                };

                var isAllowed = allowedPaths.Any(p => path?.StartsWith(p) == true);

                if (!isAllowed)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        message = "Tizimdan foydalanishdan oldin parolingizni o'zgartiring"
                    });
                    return;
                }
            }

            await _next(context);
        }
    }
}
