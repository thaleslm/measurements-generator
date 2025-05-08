using measurement_generator.Repository;
using measurement_generator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar o AppDBContext com a connection string
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o MyService como um IHostedService
builder.Services.AddScoped<CSVReaderService>();
builder.Services.AddScoped<ErpsService>();
builder.Services.AddHttpClient<DigitalTwinsClientService>();
builder.Services.AddHostedService<ScheduledTaskService>();

// Adiciona os serviços ao contêiner (DI)
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Construindo a aplicação
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
 }
// Configura o middleware para redirecionamento para HTTPS
app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    db.Database.Migrate(); // ou EnsureCreated();
}
// Usar o roteamento e o middleware de autorização
app.UseRouting();
app.UseAuthorization();

// Mapear os controllers
app.MapControllers();

// Iniciar a aplicação
app.Run();