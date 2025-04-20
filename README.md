# Zoo Management API

**Проект на ASP.NET Core Web API с использованием Domain-Driven Design и Clean Architecture**

---

## Оглавление

1. [Описание проекта](#описание-проекта)
2. [Ключевые особенности](#ключевые-особенности)
3. [Архитектура](#архитектура)
   - [Domain Layer](#domain-layer)
   - [Application Layer](#application-layer)
   - [Infrastructure Layer](#infrastructure-layer)
   - [Presentation Layer](#presentation-layer)
   - [Test Layer](#test-layer)
4. [Domain-Driven Design](#domain-driven-design)
5. [Clean Architecture](#clean-architecture)
6. [Сборка и зависимости](#сборка-и-зависимости)
7. [Запуск проекта](#запуск-проекта)
8. [Тестирование](#тестирование)
9. [Swagger / OpenAPI](#swagger--openapi)
10. [Контакты](#контакты)

---

## Описание проекта

Веб-приложение для автоматизации бизнес-процессов Московского зоопарка:
- Управление животными
- Управление вольерами
- Организация и расписание кормлений
- Сбор статистики

Реализовано на ASP.NET Core 8.0 в среде JetBrains Rider с in-memory хранением данных.

---

## Ключевые особенности

- **CRUD** для животных и вольеров
- **Перемещение** животных между вольерами
- **Расписание** и **выполнение кормлений**
- **Статистика**: общее количество животных, свободные вольеры, здоровые/больные
- **Domain Events**: `AnimalMovedEvent`, `FeedingTimeEvent`
- **Встроенное логирование** через `ILogger<T>`
- **Swagger UI** по корню `/`
- **Тестирование**: xUnit, FluentAssertions, Moq, интеграционные тесты с WebApplicationFactory
- **Покрытие**: конфигурация покрытия через `cover.runsettings` и фильтрация слоёв

---

## Архитектура

```
ZooSolution/
├── Zoo.Domain/           # Модели предметной области
├── Zoo.Application/      # Сервисы и интерфейсы бизнес-логики
├── Zoo.Infrastructure/   # In-memory репозитории и InMemoryEventBus
├── Zoo.Presentation/     # ASP.NET Core Web API (Controllers, Program.cs)
└── Zoo.Tests/            # Unit / Integration тесты
```

### Domain Layer

- Сущности: `Animal`, `Enclosure`, `FeedingSchedule`
- Value Objects: `AnimalId`, `EnclosureId`, `FeedingId`, `Enums`
- Общие базовые классы: `Entity`, `ValueObject`, `DomainEvent`
- Доменные события: `AnimalMovedEvent`, `FeedingTimeEvent`

### Application Layer

- Интерфейсы репозиториев и шины событий: `IAnimalRepository`, `IEnclosureRepository`, `IFeedingScheduleRepository`, `IEventBus`
- Сервисы:  
  - `AnimalTransferService` — перемещение животных  
  - `FeedingOrganizationService` — организация кормлений  
  - `ZooStatisticsService` — сбор статистики  

### Infrastructure Layer

- In-memory реализации репозиториев  
- `InMemoryEventBus` — хранение и публикация доменных событий в консоль  

### Presentation Layer

- ASP.NET Core Web API  
- `AnimalsController`, `EnclosuresController`, `FeedingController`, `StatisticsController`  
- Top-level `Program.cs` с:
  - DI  
  - JSON-конвертеры для `DateOnly`/`TimeOnly`  
  - Swagger + OpenAPI  
  - Seed demo data  

### Test Layer

- Unit-тесты:  
  - Domain: `AnimalTests`, `EnclosureTests`, `FeedingScheduleTests`  
  - Application: `AnimalTransferServiceTests`, `FeedingOrganizationServiceTests`, `ZooStatisticsServiceTests`  
  - Infrastructure: `InMemory*RepositoryTests`, `InMemoryEventBusTests`  
- Integration-тесты: `AnimalsControllerTests` с `WebApplicationFactory<Program>`

---

## Domain-Driven Design

- Богатая модель предметной области  
- Инкапсуляция бизнес-правил внутри сущностей  
- Использование Value Objects для идентификаторов и enum’ов  
- Публикация доменных событий при изменении состояния

---

## Clean Architecture

- Разделение на слои: Domain → Application → Infrastructure → Presentation  
- Зависимости только внутрь через интерфейсы  
- Изоляция бизнес-логики от инфраструктуры и UI

---

## Сборка и зависимости

**Требуется**: .NET 8.0 SDK, JetBrains Rider (рекомендуется).

```bash
git clone <repo-url>
cd ZooSolution
dotnet restore
dotnet build
```

---

## Запуск проекта

```bash
cd Zoo.Presentation
dotnet run
```

- Swagger UI: http://localhost:5000/ (по умолчанию `/` ведёт на Swagger)
- API endpoints: `/api/animals`, `/api/enclosures`, `/api/feeding`, `/api/stats`

---

## Тестирование

1. **Unit + Integration тесты**  
   В каталоге `Zoo.Tests` объединены все тесты.  

2. **Запуск тестов**  
   ```bash
   dotnet test Zoo.Tests --no-build
   ```

3. **Покрытие кода / Coverage**  
   Для расчёта покрытия используйте файл `cover.runsettings`, который задаёт, какие сборки учитывать:
   ```bash
   dotnet test --settings cover.runsettings --collect:"XPlat Code Coverage"
   ```
   В файле `cover.runsettings` настроены фильтры для исключения проектов:
   - `Zoo.Infrastructure*`  
   - `Zoo.Presentation*`  
   - `Zoo.Tests*`  

   Таким образом покрытие считается только по слоям Domain и Application.

---

## Swagger / OpenAPI

- Автоматически генерируется и доступен по `/swagger/v1/swagger.json`  
- UI доступен по `/` (RoutePrefix = “”)

Пример запросов:
- **POST** `/api/enclosures` — создать вольер  
- **POST** `/api/animals` — создать животное  
- **GET** `/api/animals/{id}` — получить животное  
- **POST** `/api/animals/{id}/move` — переместить животное  
- **DELETE** `/api/animals/{id}` — удалить животное  
- **POST** `/api/feeding/schedule` — добавить кормление  
- **POST** `/api/feeding/feed` — выполнить кормление  
- **GET** `/api/stats` — статистика  
