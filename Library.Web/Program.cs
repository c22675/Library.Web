using Library.Data;
using Microsoft.EntityFrameworkCore;
using Library.Grpc;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.MigrationsAssembly("Library.Web")));

builder.Services.AddHttpClient("MlService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["MlService:BaseUrl"]!);
});

builder.Services.AddGrpcClient<Library.Grpc.StatsService.StatsServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration["Grpc:BaseUrl"]!);
});

//builder.Services.AddSingleton(sp =>
//{
//    var cfg = sp.GetRequiredService<IConfiguration>();
//    var address = cfg["Grpc:BaseUrl"]!;
//    var channel = GrpcChannel.ForAddress(address);
//    return new StatsService.StatsServiceClient(channel);
//});

builder.Services.AddScoped<Library.Web.Services.MlServiceClient>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    Library.Data.SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
