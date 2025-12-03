# ?? PlayZone Backend - Quick Start Guide

## ? 5-Minute Setup

### Step 1: Start Docker (PostgreSQL + pgAdmin)
```bash
docker-compose up -d
```

**Wait 10 seconds for PostgreSQL to initialize**

### Step 2: Run the Application
```bash
dotnet run
```

**The app will automatically:**
- ? Apply database migrations
- ? Create tables with indexes
- ? Seed 8 rooms
- ? Create admin user
- ? Configure WhatsApp settings

### Step 3: Open Swagger UI
```
http://localhost:5000/swagger
```

### Step 4: Test the API

#### 1. Login as Admin
```http
POST /api/admin/login
{
  "password": "Admin@123"
}
```

**Copy the token from response**

#### 2. Click "Authorize" button in Swagger
Paste: `Bearer YOUR_TOKEN_HERE`

#### 3. Try Creating a Booking
```http
POST /api/bookings
{
  "roomId": 1,
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

## ?? That's It! Your API is Running!

---

## ?? Available Endpoints

### ?? Rooms
- `GET /api/rooms` - List all rooms
- `GET /api/rooms/available` - Get available rooms for a time slot

### ?? Bookings
- `POST /api/bookings` - Create new booking
- `GET /api/bookings/{id}` - Get booking details
- `GET /api/bookings/date/{date}` - Get bookings by date

### ?? Admin
- `POST /api/admin/login` - Login
- `GET /api/admin/stats` - Get statistics (requires auth)

### ?? WhatsApp
- `POST /api/whatsapp/send-booking` - Generate WhatsApp link
- `GET /api/whatsapp/number` - Get business number

### ? Utility
- `GET /api/time-slots` - Get available time slots (9 AM - 11 PM)
- `GET /api/health` - Health check

---

## ??? Access Database

### Option 1: pgAdmin (Web UI)
1. Open: `http://localhost:5050`
2. Login: `admin@playzone.com` / `admin123`
3. Add Server:
   - Host: `postgres`
   - Port: `5432`
   - Database: `playzone_db`
   - Username: `playzone`
   - Password: `playzone123`

### Option 2: Connection String
```
Host=localhost;Port=5432;Database=playzone_db;Username=playzone;Password=playzone123
```

---

## ?? Verify Seeded Data

### Check Rooms (Should see 8 rooms)
```http
GET /api/rooms
```

### Check Admin User Exists
```http
POST /api/admin/login
{
  "password": "Admin@123"
}
```

---

## ?? Troubleshooting

### Docker Not Starting?
```bash
# Check if ports are available
netstat -ano | findstr :5432
netstat -ano | findstr :5050

# Stop and restart
docker-compose down
docker-compose up -d
```

### Database Connection Error?
```bash
# View PostgreSQL logs
docker-compose logs postgres

# Restart containers
docker-compose restart
```

### Migration Issues?
```bash
# Remove migrations
dotnet ef migrations remove

# Recreate migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

---

## ?? Documentation

- **Full Documentation**: `README.md`
- **Docker Setup**: `README_SETUP.md`
- **Project Summary**: `PROJECT_SUMMARY.md`

---

## ?? Example: Complete Booking Flow

### 1. Get Available Time Slots
```http
GET /api/time-slots
```

### 2. Check Room Availability
```http
GET /api/rooms/available?date=2025-01-15&startTime=18:00&endTime=20:00
```

### 3. Create Booking
```http
POST /api/bookings
{
  "roomId": 3,
  "date": "2025-01-15",
  "startTime": "18:00",
  "endTime": "20:00",
  "user": {
    "name": "Ahmed",
    "phone": "01234567890"
  }
}
```

### 4. Get WhatsApp Confirmation Link
```http
POST /api/whatsapp/send-booking
{
  "bookingId": "GUID_FROM_STEP_3"
}
```

### 5. Open WhatsApp Link
The API returns a `whatsAppUrl` that opens WhatsApp with pre-filled message!

---

## ?? You're All Set!

Your PlayZone backend is now running with:
- ? PostgreSQL database
- ? 8 gaming rooms
- ? Admin authentication
- ? Booking system
- ? WhatsApp integration
- ? Complete API documentation

**Next Step**: Integrate with your React frontend! ??

---

## ?? Need Help?

Check the detailed documentation:
- `README.md` - Complete setup guide
- `PROJECT_SUMMARY.md` - Architecture overview
- Swagger UI - Interactive API testing

**Happy Coding! ??**
