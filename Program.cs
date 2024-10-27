using AppServer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//DI  ����� ����� ���� ������� + ����� ��� ���� ������   
string connectionString = builder.Configuration.GetConnectionString("SweetBoxDBContext");
builder.Services.AddDbContext<SweetBoxDBContext>(options => options.UseSqlServer(connectionString));

// ����� ����� ����
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ����� ����� ����
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
