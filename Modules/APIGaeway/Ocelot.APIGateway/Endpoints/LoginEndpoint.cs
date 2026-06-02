namespace Ocelot.APIGateway.Endpoints
{
    public static class LoginEndpoint
    {
        public static void loginEndpointFunc(this WebApplication app) 
        { 
        
            app.MapPost("/login", async (HttpContext context) =>
            {
                // Handle login logic here
                // For example, validate user credentials and generate a token
                // Example response
                var response = new { Token = "your_generated_token" };
                await context.Response.WriteAsJsonAsync(response);
            });
        }
    }
}
