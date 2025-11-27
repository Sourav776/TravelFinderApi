using TravelFinderApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

ApplicationConstants.Initialize(builder.Configuration);

builder.Services.AddSystemConfigurations();
builder.Services.AddDependencyResolver();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelFinderApi v1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseCors(options =>
        options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
