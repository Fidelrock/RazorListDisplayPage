# RazorTableDemo - MVC Pattern with API Support

This project demonstrates how Razor Pages can employ the MVC (Model-View-Controller) pattern while also providing REST API endpoints for external application communication.

## Architecture Overview

### MVC Pattern Implementation

This application follows the MVC pattern with the following structure:

#### 1. **Model Layer**
- **Models/**: Contains data models (S300TaxAuthority, UserProfile, SessionRecord)
- **Services/**: Contains business logic and data access layer
  - `ITaxAuthorityService`: Interface defining service contracts
  - `TaxAuthorityService`: Implementation of business logic

#### 2. **View Layer**
- **Pages/**: Contains Razor Pages (Views)
  - `UserProfileDisplay.cshtml`: The view for displaying user profiles
  - `ApiDocumentation.cshtml`: Documentation page for APIs

#### 3. **Controller Layer**
- **Pages/*.cshtml.cs**: PageModel classes act as controllers for Razor Pages
- **Controllers/**: Traditional API controllers for external communication
  - `UserProfileApiController`: REST API endpoints

## Key Features

### ✅ Razor Pages with MVC Pattern
- **Model**: PageModel classes contain data and business logic
- **View**: Razor (.cshtml) files handle presentation
- **Controller**: OnGet(), OnPost() methods handle user interactions

### ✅ REST API Support
- Full CRUD operations via REST endpoints
- JSON responses for external application consumption
- Proper HTTP status codes and error handling

### ✅ Service Layer Architecture
- Separation of concerns with dedicated service layer
- Dependency injection for loose coupling
- Reusable business logic across Razor Pages and APIs

### ✅ Database Integration
- Entity Framework Core for ORM
- Dapper for raw SQL queries
- SQL Server database support

## API Endpoints

### Tax Authority API (`/api/UserProfileApi`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/UserProfileApi` | Get all tax authorities (with optional filtering) |
| GET | `/api/UserProfileApi/{id}` | Get specific tax authority by ID |
| POST | `/api/UserProfileApi` | Create new tax authority |
| PUT | `/api/UserProfileApi/{id}` | Update existing tax authority |
| DELETE | `/api/UserProfileApi/{id}` | Delete tax authority |

### Query Parameters
- `clientCode`: Filter by client code (optional)
- `authorityKey`: Filter by authority key (optional)

## Project Structure

```
RazorTableDemo/
├── Controllers/                 # API Controllers
│   └── UserProfileApiController.cs
├── Data/                       # Data Access Layer
│   └── ApplicationDBContext.cs
├── Models/                     # Data Models
│   ├── S300TaxAuthority.cs
│   ├── SessionRecord.cs
│   └── UserProfile.cs
├── Pages/                      # Razor Pages (Views + Controllers)
│   ├── UserProfileDisplay.cshtml
│   ├── UserProfileDisplay.cshtml.cs
│   ├── ApiDocumentation.cshtml
│   └── ApiDocumentation.cshtml.cs
├── Services/                   # Business Logic Layer
│   ├── ITaxAuthorityService.cs
│   └── TaxAuthorityService.cs
├── Program.cs                  # Application Configuration
└── README.md                   # This file
```

## How to Use

### 1. Running the Application
```bash
dotnet run
```

### 2. Accessing Razor Pages
- **User Profiles**: `https://localhost:5001/UserProfileDisplay`
- **API Documentation**: `https://localhost:5001/ApiDocumentation`

### 3. Using the APIs
```bash
# Get all tax authorities
curl -X GET "https://localhost:5001/api/UserProfileApi"

# Create new tax authority
curl -X POST "https://localhost:5001/api/UserProfileApi" \
  -H "Content-Type: application/json" \
  -d '{"clientCode":"ABC123","authorityKey":"AUTH001","authorityName":"Test Authority"}'
```

## MVC Pattern Benefits

### 1. **Separation of Concerns**
- **Models**: Handle data and business rules
- **Views**: Handle presentation and user interface
- **Controllers**: Handle user input and coordinate between models and views

### 2. **Maintainability**
- Clear separation makes code easier to maintain
- Changes to one layer don't affect others
- Business logic is centralized in services

### 3. **Testability**
- Each layer can be tested independently
- Services can be easily mocked for unit testing
- API endpoints can be tested with tools like Postman

### 4. **Reusability**
- Services can be used by both Razor Pages and APIs
- Business logic is not duplicated
- Consistent data access patterns

## External Application Integration

External applications can integrate with this system through:

1. **REST APIs**: Standard HTTP endpoints returning JSON
2. **Authentication**: Can be added using JWT tokens or API keys
3. **Rate Limiting**: Can be implemented for API protection
4. **CORS**: Configured for cross-origin requests

## Next Steps

To enhance this architecture, consider adding:

1. **Authentication & Authorization**
2. **API Versioning**
3. **Swagger/OpenAPI Documentation**
4. **Logging and Monitoring**
5. **Caching Layer**
6. **Unit Tests**
7. **Integration Tests**

## Technologies Used

- **ASP.NET Core 8.0**
- **Razor Pages**
- **Entity Framework Core**
- **Dapper**
- **SQL Server**
- **Bootstrap** (for UI)
- **Dependency Injection**

This architecture demonstrates that Razor Pages can effectively employ the MVC pattern while providing robust API support for external application communication. 