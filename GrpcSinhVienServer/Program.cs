using GrpcSinhVienServer.Services;
using System.Text;
Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<SinhVienServiceImpl>();
app.MapGet("/", () => "Use a gRPC client to communicate.");

app.Run();
