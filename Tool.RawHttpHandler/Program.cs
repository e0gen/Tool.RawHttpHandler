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
    var raw = await GetRawBodyStringAsync(request);
    Console.WriteLine(raw);
});

app.MapPost("/bin", async (HttpRequest request) =>
{
    var raw = await GetRawBodyBytesAsync(request);
    Console.WriteLine(Encoding.UTF8.GetString(raw));
});

app.Run();

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