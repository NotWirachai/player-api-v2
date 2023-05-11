using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// กำหนดข้อมูลที่จำเป็นในการเรียกใช้ API
string titleId = "2506C";
string apiKey = "NHxrY1ZkcnNrZ3dZWkdMMEJOdmMxY0x4Rkhzc2oyaVg3TnZmK3BReFI2WnRjPXx7ImkiOiIyMDIzLTA1LTExVDA1OjE3OjIyWiIsImlkcCI6IkN1c3RvbSIsImUiOiIyMDIzLTA1LTEyVDA1OjE3OjIyWiIsImZpIjoiMjAyMy0wNS0xMVQwNToxNzoyMloiLCJ0aWQiOiJGbFQ2ZFdxdkRiYyIsImlkaSI6IjYzRkZBNEMwQjNCNUI5Q0MiLCJoIjoiMjU0Q0UzRENGMTJFMkIzIiwiZWMiOiJ0aXRsZV9wbGF5ZXJfYWNjb3VudCE2QzBERDM4QjYzOUNFRENDLzI1MDZDL0ZBNTJGNTE3Q0QyQTBEN0QvODU0OTZEMDlBOUUyNUQyMS8iLCJlaSI6Ijg1NDk2RDA5QTlFMjVEMjEiLCJldCI6InRpdGxlX3BsYXllcl9hY2NvdW50In0=";

app.UseHttpsRedirection();

app.MapGet("/player", async (HttpClient httpClient) =>
{
    // กำหนด URL ของ API
    string url = $"https://{titleId}.playfabapi.com/Inventory/GetInventoryCollectionIds";

    // กำหนดข้อมูลใน request body
    var data = new Dictionary<string, object>
    {
        { "Count", 1 }
    };

    // แปลงข้อมูลใน request body เป็น JSON format
    string json = JsonSerializer.Serialize(data);

    // กำหนด header ของ HTTP request
    httpClient.DefaultRequestHeaders.Add("X-EntityToken", apiKey);

    // ส่ง HTTP GET request ไปยัง API ผ่าน HttpClient
    HttpResponseMessage response = await httpClient.GetAsync(url);

    // ดึงข้อมูลใน response body และแปลงเป็น JSON object
    string responseBody = await response.Content.ReadAsStringAsync();
    dynamic jsonResult = JsonSerializer.Deserialize<dynamic>(responseBody);

    // ส่ง JSON response กลับไปหา client
    return Results.Json(jsonResult);
})
.WithName("")
.WithOpenApi();

app.Run();