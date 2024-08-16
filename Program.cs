using Microsoft.AspNetCore.Mvc.Formatters;
using TestApplication.Services.Implementations;
using TestApplication.Services.Interfaces;
using TestApplication.Tuls;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
    options.OutputFormatters.Add(new CsvOutputFormatter());
}).AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// моковые данные
var balanceFilePath = "jsonmodels/balance_202105270825.json";
var paymentFilePath = "jsonmodels/payment_202105270827.json";

builder.Services.AddSingleton<IBalanceService>(sp =>
    new BalanceService(balanceFilePath, paymentFilePath));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
