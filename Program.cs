using measurement_generator.Repository;
using measurement_generator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Registrar o AppDBContext com a connection string
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Adiciona o MyService como um IHostedService
builder.Services.AddScoped<CSVReaderService>();
//builder.Services.AddHostedService<ScheduledTaskService>();

// Adiciona os serviços ao contêiner (DI)
builder.Services.AddControllers();

// Construindo a aplicação
var app = builder.Build();

// Configura o middleware para redirecionamento para HTTPS
app.UseHttpsRedirection();

// Usar o roteamento e o middleware de autorização
app.UseRouting();
app.UseAuthorization();

// Mapear os controllers
app.MapControllers();

// Iniciar a aplicação
app.Run();