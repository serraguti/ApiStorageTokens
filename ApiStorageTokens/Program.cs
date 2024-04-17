using ApiStorageTokens.Services;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ServiceSasToken>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

//AQUI SE VAN MAPEANDO LOS DISTINTOS METODOS NECESARIOS
app.MapGet("/testapi", () =>
{
    return "Testing Minimal Api";
});

//QUEREMOS UN METODO PARA GENERAR EL TOKEN Y QUE RECIBA UN CURSO
//LOGICAMENTE PERO NO PODEMOS UTILIZAR LAS PALABRAS 
//RESERVADAS [action] o [controller]
//NECESITAMOS EL SERVICE Y TENEMOS DOS FORMAS DE RECUPERARLO
//DENTRO DEL METODO BUSCANDO EL SERVICIO CON Services.GetService<>
//UTILIZANDO LA INYECCION
app.MapGet("/token/{curso}", (string curso,
    ServiceSasToken service) =>
{
    string token = service.GenerateToken(curso);
    return new { token = token };
});

app.Run();

