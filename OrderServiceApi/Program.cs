using Microsoft.EntityFrameworkCore;
using OrderServiceApi.Data;
using OrderServiceApi.Repositories;
using OrderServiceApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(option =>
{
    
    option.JsonSerializerOptions.IgnoreNullValues = true;
    option.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var paymentBaseUrl = builder.Configuration["PaymentService:BaseUrl"] ?? "http://localhost:5093";
builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri(paymentBaseUrl.TrimEnd('/') + "/");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.MapGet("/", () => "OrderServiceApi is running! v2");

app.Run();
