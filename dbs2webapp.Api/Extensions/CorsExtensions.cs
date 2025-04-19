namespace dbs2webapp.Api.Extensions
{
    public static class CorsExtensions
    {
        private const string CorsPolicyName = "CorsPolicy";

        public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration config)
        {
            var allowedOrigins = config.GetSection("Cors:AllowedOrigins").Get<string[]>()
                                 ?? new[] { "http://localhost:5276", "http://localhost:3000" };

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // important if using auth
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            return app.UseCors(CorsPolicyName);
        }
    }
}
