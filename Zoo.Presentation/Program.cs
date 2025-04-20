using Zoo.Application.Services;
using Zoo.Infrastructure.EventBus;
using Zoo.Infrastructure.Repositories;
using Zoo.Application.Interfaces;
using Zoo.Domain.Entities;
using Zoo.Domain.ValueObjects;

var builder = WebApplication.CreateBuilder(args);

// DI – Infrastructure
builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
builder.Services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// DI – Application
builder.Services.AddScoped<AnimalTransferService>();
builder.Services.AddScoped<FeedingOrganizationService>();
builder.Services.AddScoped<ZooStatisticsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

// Seed demo data
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