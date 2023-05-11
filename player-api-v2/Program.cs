using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

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
//string playFabId = "FA52F517CD2A0D7D";
string apiKey = "NHw5dmRUeXcrK1ljYXZSYmhETlMzMGd1U0U3OHozRVhUTjgycFA5bitRZnA0PXx7ImkiOiIyMDIzLTA1LTExVDA3OjE2OjQxWiIsImlkcCI6IkN1c3RvbSIsImUiOiIyMDIzLTA1LTEyVDA3OjE2OjQxWiIsImZpIjoiMjAyMy0wNS0xMVQwNzoxNjo0MVoiLCJ0aWQiOiJKaExrbDlrNE11WSIsImlkaSI6Ik5vVFRZODg4IiwiaCI6IjI1NENFM0RDRjEyRTJCMyIsImVjIjoidGl0bGVfcGxheWVyX2FjY291bnQhNkMwREQzOEI2MzlDRURDQy8yNTA2Qy9GNkExN0YzQjI4QTk4RUI0LzYzRkZBNEMwQjNCNUI5Q0MvIiwiZWkiOiI2M0ZGQTRDMEIzQjVCOUNDIiwiZXQiOiJ0aXRsZV9wbGF5ZXJfYWNjb3VudCJ9";

app.UseHttpsRedirection();


Console.WriteLine("titleId =>> "+ "2506C"+ titleId);

app.MapGet("/player", async (HttpClient httpClient) =>
{
    // กำหนด URL ของ API
    string url = $"https://{titleId}.playfabapi.com/Inventory/GetInventoryCollectionIds";

    // กำหนดข้อมูลใน request body
    var data = new Dictionary<string, object>
    {
        { "Count", 1 },
    };

    // แปลงข้อมูลใน request body เป็น JSON format
    string json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    // กำหนด header ของ HTTP request
    httpClient.DefaultRequestHeaders.Add("X-EntityToken", apiKey);

    // ส่ง HTTP POST request ไปยัง API ผ่าน HttpClient
    HttpResponseMessage response = await httpClient.PostAsync(url, content);
    // ดึงข้อมูลใน response body และแปลงเป็น JSON object
    string responseBody = await response.Content.ReadAsStringAsync();
    dynamic jsonResult = JsonSerializer.Deserialize<dynamic>(responseBody);
    Console.WriteLine("response =>> " + responseBody);

    // ส่ง JSON response กลับไปหา client
    return Results.Json(jsonResult);
})
.WithName("Player")
.WithOpenApi();

app.Run();
