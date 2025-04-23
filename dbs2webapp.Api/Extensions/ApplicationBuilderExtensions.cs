namespace Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseAppSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dbs2WebApp API v1");
                c.RoutePrefix = string.Empty; // Swagger at root
            });

            return app;
        }
    }
}