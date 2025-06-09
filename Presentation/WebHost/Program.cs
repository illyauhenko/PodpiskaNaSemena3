using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// ��������� DbContext � ������������ � PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// �������� ������ �������, ��������, controllers ��� Razor Pages, ���� �����
builder.Services.AddControllers();

var app = builder.Build();

// �������� �������� ��� ������ ����������
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("�������� ��������� �������.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"������ ��� ���������� ��������: {ex.Message}");
        throw;
    }
}

app.MapControllers();

// ������������ middleware � � �������, ����������� ���������, ����� ������ �������
app.Run();
