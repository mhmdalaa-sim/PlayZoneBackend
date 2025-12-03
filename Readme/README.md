# PlayZone Backend API - Complete Setup Guide

## ?? Overview

This is a complete ASP.NET Core 8 Web API for the PlayZone PlayStation café booking system with PostgreSQL database.

## ?? Project Structure

```
PlayZone/
??? Configuration/     # Configuration classes (JWT, Admin, CORS, etc.)
??? Controllers/     # API Controllers
?   ??? AdminController.cs
?   ??? BlockedSlotsController.cs
?   ??? BookingsController.cs
?   ??? RoomsController.cs
?   ??? UtilityController.cs
?   ??? WhatsAppController.cs
??? Data/ # DbContext and database configuration
?   ??? PlayZoneDbContext.cs
??? DTOs/              # Data Transfer Objects
?   ??? AdminDto.cs
?   ??? ApiResponse.cs
?   ??? BlockedSlotDto.cs
?   ??? BookingDto.cs
?   ??? RoomDto.cs
?   ??? UtilityDto.cs
?   ??? WhatsAppDto.cs
??? Middleware/    # Custom middleware
?   ??? ErrorHandlingMiddleware.cs
??? Migrations/  # EF Core migrations
??? Models/     # Entity models
?   ??? AdminUser.cs
? ??? BlockedSlot.cs
?   ??? Booking.cs
?   ??? Room.cs
?   ??? WhatsAppConfig.cs
??? Repositories/         # Data access layer
?   ??? IAdminRepository.cs & AdminRepository.cs
?   ??? IBlockedSlotRepository.cs & BlockedSlotRepository.cs
?   ??? IBookingRepository.cs & BookingRepository.cs
?   ??? IRoomRepository.cs & RoomRepository.cs
?   ??? IWhatsAppConfigRepository.cs & WhatsAppConfigRepository.cs
??? Services/            # Business logic layer
?   ??? IAdminService.cs & AdminService.cs
?   ??? IBlockedSlotService.cs & BlockedSlotService.cs
?   ??? IBookingService.cs & BookingService.cs
?   ??? IRoomService.cs & RoomService.cs
? ??? IWhatsAppService.cs & WhatsAppService.cs
??? Utilities/ # Helper utilities
?   ??? IValidationService.cs & ValidationService.cs
?   ??? JwtHelper.cs
?   ??? TimeHelper.cs
?   ??? WhatsAppHelper.cs
??? docker-compose.yml      # Docker configuration for PostgreSQL & pgAdmin
??? .env          # Environment variables (not in Git)
??? .env.example     # Environment variables template
??? appsettings.json        # Application configuration
??? Program.cs          # Application entry point
??? README_SETUP.md       # Docker setup guide
```

## ?? Quick Start

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- Git

### 1. Clone & Navigate

```bash
cd D:\MyWork\brave\Backend\PlayZone
```

### 2. Start Docker Containers

```bash
docker-compose up -d
```

This starts:
- **PostgreSQL** on `localhost:5432`
- **pgAdmin** on `localhost:5050`

### 3. Verify Environment

Make sure `.env` file exists with correct values:

```env
POSTGRES_USER=playzone
POSTGRES_PASSWORD=playzone123
POSTGRES_DB=playzone_db
DATABASE_URL=Host=localhost;Port=5432;Database=playzone_db;Username=playzone;Password=playzone123
JWT_SECRET=YourSuperSecretKeyForJWTTokenGeneration123!@#
ADMIN_PASSWORD=Admin@123
WHATSAPP_NUMBER=+201234567890
```

### 4. Run the Application

```bash
dotnet run
```

The API will start at:
- **HTTP**: `http://localhost:5000`
- **Swagger**: `http://localhost:5000/swagger`

The application will **automatically**:
? Apply database migrations
? Seed initial data (8 rooms, admin user, WhatsApp config)

## ?? Seeded Data

After first run, your database will contain:

### Rooms (8 total)
- Room 1 through Room 8
- All available by default

### Admin User
- **Username**: `admin`
- **Password**: `Admin@123`

### WhatsApp Config
- **Default Number**: `+201234567890`

## ?? Authentication

### Login as Admin

```bash
POST http://localhost:5000/api/admin/login
Content-Type: application/json

{
  "password": "Admin@123"
}
```

Response:
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expiresAt": "2025-01-11T12:00:00Z",
    "username": "admin"
  }
}
```

### Use Token in Requests

```bash
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

## ?? API Endpoints

### Public Endpoints (No Auth Required)

#### Rooms
- `GET /api/rooms` - Get all rooms
- `GET /api/rooms/{id}` - Get room by ID
- `GET /api/rooms/available?date={date}&startTime={time}&endTime={time}` - Get available rooms
- `GET /api/rooms/{id}/status?date={date}&startTime={time}&endTime={time}` - Get room status

#### Bookings
- `GET /api/bookings/{id}` - Get booking by ID
- `POST /api/bookings` - Create new booking
- `GET /api/bookings/date/{date}` - Get bookings by date
- `GET /api/bookings/room/{roomId}` - Get bookings by room

#### WhatsApp
- `POST /api/whatsapp/send-booking` - Generate WhatsApp booking link
- `GET /api/whatsapp/number` - Get business WhatsApp number

#### Utility
- `GET /api/time-slots` - Get available time slots
- `GET /api/health` - Health check

#### Admin Auth
- `POST /api/admin/login` - Admin login

### Protected Endpoints (Require Admin Auth)

#### Rooms
- `PUT /api/rooms/{id}/availability` - Enable/disable room

#### Bookings
- `GET /api/bookings` - Get all bookings (with pagination & filters)
- `DELETE /api/bookings/{id}` - Delete booking

#### Blocked Slots
- `GET /api/blocked-slots` - Get all blocked slots
- `GET /api/blocked-slots/{id}` - Get blocked slot by ID
- `POST /api/blocked-slots` - Create blocked slot
- `DELETE /api/blocked-slots/{id}` - Delete blocked slot
- `GET /api/blocked-slots/room/{roomId}/date/{date}` - Get blocked slots by room & date

#### Admin
- `POST /api/admin/logout` - Admin logout
- `GET /api/admin/stats` - Get booking statistics

#### WhatsApp
- `PUT /api/whatsapp/number` - Update business WhatsApp number

## ?? Example API Calls

### Create a Booking

```bash
POST http://localhost:5000/api/bookings
Content-Type: application/json

{
  "roomId": 3,
  "date": "2025-01-15",
  "startTime": "18:00",
  "endTime": "20:00",
  "user": {
    "name": "Ahmed Mohamed",
    "phone": "01234567890"
  },
  "notes": "Birthday party"
}
```

### Get Available Rooms

```bash
GET http://localhost:5000/api/rooms/available?date=2025-01-15&startTime=18:00&endTime=20:00
```

### Block a Time Slot (Admin)

```bash
POST http://localhost:5000/api/blocked-slots
Authorization: Bearer {your_token}
Content-Type: application/json

{
  "roomId": 5,
  "date": "2025-01-15",
  "startTime": "14:00",
  "endTime": "16:00",
  "reason": "Maintenance"
}
```

### Get Booking Statistics (Admin)

```bash
GET http://localhost:5000/api/admin/stats
Authorization: Bearer {your_token}
```

## ??? Database Management

### Access pgAdmin

1. Open browser: `http://localhost:5050`
2. Login:
   - Email: `admin@playzone.com`
   - Password: `admin123`

3. Add Server:
 - Host: `postgres` (Docker) or `localhost` (from host)
   - Port: `5432`
   - Database: `playzone_db`
   - Username: `playzone`
   - Password: `playzone123`

### Manual Database Operations

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# View migrations
dotnet ef migrations list
```

## ??? Architecture

### Layered Architecture

```
???????????????????????????????????????
?        Controllers (HTTP)        ?
?  (API Endpoints, Request/Response)  ?
???????????????????????????????????????
          ?
???????????????????????????????????????
?   Services (Business)         ?
?   (Business Logic, Validation)      ?
???????????????????????????????????????
     ?
???????????????????????????????????????
?      Repositories (Data Access)     ?
?    (Database Queries, CRUD)  ?
???????????????????????????????????????
            ?
???????????????????????????????????????
?     DbContext (EF Core)          ?
?       (PostgreSQL)        ?
???????????????????????????????????????
```

### Key Design Patterns

- ? **Repository Pattern** - Data access abstraction
- ? **Service Layer** - Business logic separation
- ? **Dependency Injection** - Loose coupling
- ? **DTO Pattern** - API contract isolation
- ? **Middleware Pattern** - Cross-cutting concerns

## ?? Security Features

- ? JWT Authentication
- ? Password Hashing (BCrypt)
- ? Role-Based Authorization
- ? CORS Configuration
- ? Global Error Handling
- ? Input Validation

## ?? Testing

### Test with Swagger

Open `http://localhost:5000/swagger` and test all endpoints interactively.

### Test with curl/Postman

Import the API endpoints into Postman or use curl:

```bash
# Health check
curl http://localhost:5000/api/health

# Get all rooms
curl http://localhost:5000/api/rooms

# Admin login
curl -X POST http://localhost:5000/api/admin/login \
  -H "Content-Type: application/json" \
  -d '{"password":"Admin@123"}'
```

## ?? Dependencies

### NuGet Packages

- `Microsoft.EntityFrameworkCore` (8.0.0)
- `Microsoft.EntityFrameworkCore.Design` (8.0.0)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (8.0.0)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.0)
- `System.IdentityModel.Tokens.Jwt` (7.0.3)
- `BCrypt.Net-Next` (4.0.3)
- `FluentValidation.AspNetCore` (11.3.0)
- `Serilog.AspNetCore` (8.0.0)
- `Swashbuckle.AspNetCore` (6.6.2)

## ?? Troubleshooting

### Port Already in Use

Change ports in `.env`:
```env
POSTGRES_PORT=5433
PGADMIN_PORT=5051
```

Update `appsettings.json` connection string accordingly.

### Database Connection Failed

1. Ensure Docker containers are running: `docker ps`
2. Check container logs: `docker-compose logs postgres`
3. Verify connection string in `appsettings.json`

### Migrations Not Applied

Run manually:
```bash
dotnet ef database update
```

## ?? Next Steps

- [ ] Add unit tests
- [ ] Add integration tests
- [ ] Implement email notifications
- [ ] Add payment integration
- [ ] Implement customer accounts
- [ ] Add real-time updates (SignalR)
- [ ] Deploy to production

## ?? Contributing

1. Create feature branch
2. Make changes
3. Test locally
4. Submit pull request

## ?? License

This project is part of the PlayZone booking system.

## ????? Developer

Developed for PlayZone PlayStation Café Management System

---

**Happy Coding! ??**
