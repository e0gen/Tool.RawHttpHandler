using Microsoft.AspNetCore.Components.Web;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/txt", async (HttpRequest request) =>
{
    foreach (var header in request.Headers)
    {
        string headerName = header.Key;
        string headerContent = string.Join(",", header.Value.ToArray());
        Console.WriteLine($"{headerName}:{headerContent}");
    }
    var rawBody = await GetRawBodyStringAsync(request);
    Console.WriteLine($"Body:{rawBody}");
});

app.MapPost("/bin", async (HttpRequest request) =>
{
    var raw = await GetRawBodyBytesAsync(request);
    Console.WriteLine(Encoding.UTF8.GetString(raw));
});

app.Run();

Console.ReadKey();

async Task<string> GetRawBodyStringAsync(HttpRequest request, Encoding? encoding = null)
{
    if (encoding == null)
        encoding = Encoding.UTF8;

    using (StreamReader reader = new StreamReader(request.Body, encoding))
        return await reader.ReadToEndAsync();
}

async Task<byte[]> GetRawBodyBytesAsync(HttpRequest request)
{
    using (var ms = new MemoryStream(2048))
    {
        await request.Body.CopyToAsync(ms);
        return ms.ToArray();
    }
}