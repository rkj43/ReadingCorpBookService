using Microsoft.EntityFrameworkCore;
using ReadingCorp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<BookContext>(opt =>
    opt.UseInMemoryDatabase("BookList"));
// Add CORS services. https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
