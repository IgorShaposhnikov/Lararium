# 🛠 Backend Architecture & Guidelines

The backend of Lararium is built with **C# / ASP.NET Core (.NET 10)**. Instead of jumping straight into microservices, I chose a strict **Modular Monolith** architecture. This approach keeps deployment simple (one process) while strictly enforcing boundaries between domains, making the system scalable and easy to maintain.

## 🏗️ Solution Structure

The project is sliced into horizontal layers and vertical feature modules:

* **`Lararium.API`**: The main host project. It contains `Program.cs`, global middlewares (exception handling, CORS, Swagger), and wires everything together. **No business logic lives here.**
* **`Lalarium.Core`**: Shared domain abstractions. Contains base models (`LarariumUser`), interfaces (`IUser`), persistence abstractions (`IDataStore`, `IUserDataStore`), and the core module registration system (`IModuleInitializer`).
* **`Lararium.Persistence`**: The unified data access layer. Contains the `AppDbContext`, EF Core migrations, and generic implementations of `DataStoreBase`.
* **Feature Modules** (`Lalarium.Authorization.JWT`, `Lararium.Video`, `Lararium.Media`): Independent class libraries containing their own Controllers, Services, Models, and domain logic.

## 🧩 The Module System (`IModuleInitializer`)

To keep modules completely decoupled, the API host does not manually reference every service. Instead, each feature module must implement an `IModuleInitializer`:

```csharp
public class ModuleInitializer : IModuleInitializer
{
    public ModuleMetadata GetMetadata() => new()
    {
        Id = Guid.Parse("..."),
        Name = "Lararium Video",
        Assembly = typeof(ModuleInitializer).Assembly,
        HasApiControllers = true,
        Version = "1.0.0"
    };

    public IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register module-specific services, options, and logic here
        services.AddScoped<IVideoEncoder<HlsEncoder>, HlsEncoder>();
        return services;
    }
}
```
At startup, `Lararium.API` uses reflection (`ModuleRegistrationExtensions`) to find all classes implementing `IModuleInitializer` and dynamically registers their services and controllers.

## 💾 Database & Data Access

* **EF Core & PostgreSQL:** I use Postgres. The `AppDbContext` is configured with `SnakeCaseNamingConvention` (so `VideoEntityId` automatically becomes `video_entity_id` in the DB).
* **DataStores (Repository Pattern):** Direct `DbContext` usage in services and controllers is **strictly forbidden**. I use the `IDataStore<TEntity, TId>` interface. 
* **Optimized Reads:** DataStore read methods include an `asNoTracking` parameter (default `true`) to optimize performance for read-only scenarios.
* **Specialized Stores:** For entities requiring specific DB queries (like users), I create specialized interfaces (e.g., `IUserDataStore` extending `IDataStore<LarariumUser, Guid>`).
  
All DataStores (both generic and specialized) are automatically registered into the DI container via reflection in `DataStoresServiceCollectionExtensions`.

## 🔐 Identity & Authorization
Authentication logic is isolated in the `Lalarium.Authorization` module. 
* **Business Logic:** I use `IJwtIdentityService` for login and registration operations, meaning the API endpoints act solely as thin orchestrators.
* **Tokens:** The system uses JWT for authorization and Redis/Garnet for caching Refresh Tokens.
* **Hashing:** Password hashing is delegated to ASP.NET Core's built-in `IPasswordHasher<LarariumUser>`.

## 🎥 Video Processing
Video handling is one of the heaviest parts of the app. Instead of basic file serving, I use **FFmpeg (via `FFMpegCore`)** to chunk videos into HLS streams (`.m3u8` and `.ts` files). 
* Heavy processing jobs should be offloaded to background queues so they don't block the HTTP request threads.

## 🚦 API Versioning

The API uses `Asp.Versioning.Http`. All controllers must be versioned:
```csharp
[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class VideoController : ControllerBase { ... }
```
You can pass the version via URL segment, query string (`?api-version=1.0`), or header (`X-Version: 1.0`).

---

### 🚀 How to create a new module:
1. Create a new Class Library (e.g., `Lararium.Drive`).
2. Add a reference to `Lalarium.Core`.
3. Create a `ModuleInitializer.cs` and implement `IModuleInitializer`.
4. Add your Controllers, Models, and Services inside this project.
5. If your module needs its own database tables, add the Models to `AppDbContext` and create DataStores inheriting from `DataStoreBase` in `Lararium.Persistence`.
6. Add the project reference to `Lararium.API` and `Lararium.slnx`. The API host will automatically pick it up!