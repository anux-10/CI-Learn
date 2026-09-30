using Microsoft.EntityFrameworkCore;
using PaymentServiceApi.Data;
using PaymentServiceApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=payments.db"));

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.MapGet("/", () => "PaymentServiceApi is running! v2");

app.Run();
