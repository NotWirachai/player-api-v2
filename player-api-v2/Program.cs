using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

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
app.UseHttpsRedirection();

app.MapGet("/player", async (HttpClient httpClient, [FromHeader(Name = "X-EntityToken")] string entityToken) =>
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
    httpClient.DefaultRequestHeaders.Add("X-EntityToken", entityToken);

    // ส่ง HTTP POST request ไปยัง API ผ่าน HttpClient
    HttpResponseMessage response = await httpClient.PostAsync(url, content);
    // ดึงข้อมูลใน response body และแปลงเป็น JSON object
    string responseBody = await response.Content.ReadAsStringAsync();
    dynamic jsonResult = JsonSerializer.Deserialize<dynamic>(responseBody);
    Console.WriteLine("response =>> " + responseBody);
    Console.WriteLine("jsonResult =>> " + jsonResult);
    // ส่ง JSON response กลับไปหา client
    return Results.Json(jsonResult);
})
.WithName("Player")
.WithOpenApi();

app.Run();
