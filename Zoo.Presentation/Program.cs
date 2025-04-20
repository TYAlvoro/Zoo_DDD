using System.Text.Json;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Zoo.Application.Interfaces;
using Zoo.Application.Services;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;
using Zoo.Infrastructure.EventBus;
using Zoo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. DI – Infrastructure
builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// 2. DI – Application
builder.Services.AddScoped<AnimalTransferService>();
builder.Services.AddScoped<FeedingOrganizationService>();
builder.Services.AddScoped<ZooStatisticsService>();

// 3. Controllers + JSON‑options
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
    {
        // сериализация enum как строки
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // конвертеры для DateOnly / TimeOnly
        opts.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

// 4. SwaggerGen с кастомными schemaId и MapType
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // уникальные имена схем для вложенных DTO
    c.CustomSchemaIds(type => type.FullName!.Replace("+", "."));
    
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Zoo API", Version = "v1" });

    // описываем DateOnly → string (date)
    c.MapType<DateOnly>(() =>
        new OpenApiSchema { Type = "string", Format = "date" });
    // описываем TimeOnly → string (time)
    c.MapType<TimeOnly>(() =>
        new OpenApiSchema { Type = "string", Format = "time" });
});

var app = builder.Build();

// 5. Dev‑exception page
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// 6. Swagger UI ДО контроллеров
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "";  // Swagger по корню http://localhost:5000/
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo API V1");
});

// 7. Контроллеры
app.MapControllers();

// 8. Seed demo data
using(var scope = app.Services.CreateScope())
{
    var animals = scope.ServiceProvider.GetRequiredService<IAnimalRepository>();
    var enclosures = scope.ServiceProvider.GetRequiredService<IEnclosureRepository>();

    var e1 = new Enclosure(EnclosureId.New(), EnclosureType.Carnivore, 100, 2);
    await enclosures.AddAsync(e1);

    var a1 = new Animal(
        AnimalId.New(), "Lion", "Simba", new DateOnly(2021, 6, 1),
        Gender.Male, "Meat", AnimalStatus.Healthy, e1.Id);

    e1.AddAnimal(a1);
    await animals.AddAsync(a1);
    await animals.SaveChangesAsync();
    await enclosures.SaveChangesAsync();
}

app.Run();


// ==========================================
// JSON‑конвертеры для DateOnly и TimeOnly
// ==========================================
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateOnly.ParseExact(reader.GetString()!, Format);
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(Format));
}

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm:ss";
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        TimeOnly.ParseExact(reader.GetString()!, Format);
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(Format));
}
