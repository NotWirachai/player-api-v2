using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    string url = $"https://{titleId}.playfabapi.com/Inventory/GetInventoryCollectionIds";

    var data = new Dictionary<string, object>
    {
        { "Count", 1 },
    };

    string json = JsonSerializer.Serialize(data);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    httpClient.DefaultRequestHeaders.Add("X-EntityToken", entityToken);

    HttpResponseMessage response = await httpClient.PostAsync(url, content);
    string responseBody = await response.Content.ReadAsStringAsync();
    dynamic jsonResult = JsonSerializer.Deserialize<dynamic>(responseBody);

    if (jsonResult.ValueKind == JsonValueKind.Object)
    {
        if (jsonResult.TryGetProperty("data", out JsonElement dataElement) && dataElement.ValueKind == JsonValueKind.Object)
        {
            if (dataElement.TryGetProperty("CollectionIds", out JsonElement collectionIdsElement) && collectionIdsElement.ValueKind == JsonValueKind.Array && collectionIdsElement.GetArrayLength() > 0)
            {
                // CollectionIds has values
                Console.WriteLine("CollectionIds has values");
                // return new JsonResult(collectionIdsElement);
                return Results.Json(jsonResult);
            }
            else
            {
                // CollectionIds is empty
                var requestBody = new Dictionary<string, object>
                    {
                        { "Amount", 1 },
                        { "Entity", new Dictionary<string, object>
                            {
                                { "Id", "5C22B048B1D16E86" },
                                { "Type", "title_player_account" },
                                { "TypeString", "title_player_account" }
                            }
                        },
                        { "Item", new Dictionary<string, object>
                            {
                                { "Id", "e9086e27-c51c-4072-ba27-621698b81fe1" }
                            }
                        },
                        {"CollectionId","State1-0" }
                    };

                string collectionJson = JsonSerializer.Serialize(requestBody);
                var collectionContent = new StringContent(collectionJson, Encoding.UTF8, "application/json");

                string urlCollectionIds = $"https://{titleId}.playfabapi.com/Inventory/AddInventoryItems";
                HttpResponseMessage responseCollectionIds = await httpClient.PostAsync(urlCollectionIds, collectionContent);
                string responseBodyCollectionIds = await responseCollectionIds.Content.ReadAsStringAsync();
                dynamic jsonResultCollectionIds = JsonSerializer.Deserialize<dynamic>(responseBody);
                return Results.Json(jsonResultCollectionIds);
               // return new JsonResult("CollectionIds is empty");
            }
        }
        else
        {
            // CollectionIds is missing
            return new JsonResult("CollectionIds is missing");

        }
    }
    else
    {
        // Invalid response format
        return new JsonResult("Invalid response format");
    }

})
.WithName("Player")
.WithOpenApi();

app.Run();
