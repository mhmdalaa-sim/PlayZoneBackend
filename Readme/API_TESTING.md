# ?? PlayZone API Testing Collection

## Base URL
```
http://localhost:5000
```

---

## ?? Authentication

### Admin Login
```http
POST /api/admin/login
Content-Type: application/json

{
  "password": "Admin@123"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2025-01-11T12:00:00Z",
    "username": "admin"
  }
}
```

**Use token in subsequent requests:**
```http
Authorization: Bearer {token}
```

---

## ?? Rooms

### Get All Rooms
```http
GET /api/rooms
```

### Get Room by ID
```http
GET /api/rooms/1
```

### Get Available Rooms for Time Slot
```http
GET /api/rooms/available?date=2025-01-15&startTime=18:00&endTime=20:00
```

### Get Room Status
```http
GET /api/rooms/3/status?date=2025-01-15&startTime=18:00&endTime=20:00
```

### Update Room Availability (Admin Only)
```http
PUT /api/rooms/5/availability
Authorization: Bearer {token}
Content-Type: application/json

{
  "available": false
}
```

---

## ?? Bookings

### Create Booking
```http
POST /api/bookings
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
  "notes": "Birthday party - need balloons"
}
```

### Get Booking by ID
```http
GET /api/bookings/{booking-id}
```

### Get All Bookings (Admin Only)
```http
GET /api/bookings?page=1&pageSize=20
Authorization: Bearer {token}
```

### Get All Bookings with Filters (Admin Only)
```http
GET /api/bookings?date=2025-01-15&roomId=3&page=1&pageSize=20
Authorization: Bearer {token}
```

### Get Bookings by Date
```http
GET /api/bookings/date/2025-01-15
```

### Get Bookings by Room
```http
GET /api/bookings/room/3
```

### Delete Booking (Admin Only)
```http
DELETE /api/bookings/{booking-id}
Authorization: Bearer {token}
```

---

## ?? Blocked Slots

### Get All Blocked Slots (Admin Only)
```http
GET /api/blocked-slots
Authorization: Bearer {token}
```

### Get Blocked Slot by ID (Admin Only)
```http
GET /api/blocked-slots/{slot-id}
Authorization: Bearer {token}
```

### Create Blocked Slot (Admin Only)
```http
POST /api/blocked-slots
Authorization: Bearer {token}
Content-Type: application/json

{
  "roomId": 5,
  "date": "2025-01-15",
  "startTime": "14:00",
  "endTime": "16:00",
  "reason": "Maintenance - AC repair"
}
```

### Get Blocked Slots by Room and Date (Admin Only)
```http
GET /api/blocked-slots/room/5/date/2025-01-15
Authorization: Bearer {token}
```

### Delete Blocked Slot (Admin Only)
```http
DELETE /api/blocked-slots/{slot-id}
Authorization: Bearer {token}
```

---

## ????? Admin

### Login
```http
POST /api/admin/login
Content-Type: application/json

{
  "password": "Admin@123"
}
```

### Logout (Admin Only)
```http
POST /api/admin/logout
Authorization: Bearer {token}
```

### Get Statistics (Admin Only)
```http
GET /api/admin/stats
Authorization: Bearer {token}
```

**Response includes:**
- Total bookings
- Bookings today/week/month
- Room statistics
- Revenue

---

## ?? WhatsApp

### Generate WhatsApp Booking Link
```http
POST /api/whatsapp/send-booking
Content-Type: application/json

{
  "bookingId": "{booking-guid}"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "whatsAppUrl": "https://wa.me/201234567890?text=...",
    "message": "?? *PlayZone Booking Confirmation*\n\n..."
  }
}
```

### Get Business WhatsApp Number
```http
GET /api/whatsapp/number
```

### Update Business WhatsApp Number (Admin Only)
```http
PUT /api/whatsapp/number
Authorization: Bearer {token}
Content-Type: application/json

{
  "businessNumber": "+201234567890"
}
```

---

## ? Utility

### Get Available Time Slots
```http
GET /api/time-slots
```

**Returns:**
```json
{
  "success": true,
  "data": [
    { "time": "09:00", "display": "09:00" },
    { "time": "09:30", "display": "09:30" },
    ...
    { "time": "23:00", "display": "23:00" }
  ]
}
```

### Health Check
```http
GET /api/health
```

**Returns:**
```json
{
  "status": "healthy",
  "timestamp": "2025-01-10T12:00:00Z",
  "version": "1.0.0",
  "environment": "Development"
}
```

---

## ?? Test Scenarios

### Scenario 1: Complete Booking Flow

1. **Get available time slots**
```http
GET /api/time-slots
```

2. **Check available rooms**
```http
GET /api/rooms/available?date=2025-01-15&startTime=18:00&endTime=20:00
```

3. **Create booking**
```http
POST /api/bookings
{
  "roomId": 3,
  "date": "2025-01-15",
  "startTime": "18:00",
  "endTime": "20:00",
  "user": { "name": "Ahmed", "phone": "01234567890" }
}
```

4. **Generate WhatsApp link**
```http
POST /api/whatsapp/send-booking
{
  "bookingId": "{from-step-3}"
}
```

### Scenario 2: Admin Operations

1. **Login**
```http
POST /api/admin/login
{ "password": "Admin@123" }
```

2. **View all bookings**
```http
GET /api/bookings?page=1&pageSize=20
Authorization: Bearer {token}
```

3. **Disable room for maintenance**
```http
PUT /api/rooms/5/availability
Authorization: Bearer {token}
{ "available": false }
```

4. **Block time slot**
```http
POST /api/blocked-slots
Authorization: Bearer {token}
{
  "roomId": 5,
  "date": "2025-01-15",
  "startTime": "14:00",
  "endTime": "16:00",
  "reason": "Maintenance"
}
```

5. **Get statistics**
```http
GET /api/admin/stats
Authorization: Bearer {token}
```

### Scenario 3: Conflict Testing

1. **Create first booking**
```http
POST /api/bookings
{
  "roomId": 1,
  "date": "2025-01-15",
  "startTime": "18:00",
  "endTime": "20:00",
  "user": { "name": "User1", "phone": "01111111111" }
}
```

2. **Try overlapping booking (should fail)**
```http
POST /api/bookings
{
  "roomId": 1,
  "date": "2025-01-15",
  "startTime": "19:00",
  "endTime": "21:00",
  "user": { "name": "User2", "phone": "01222222222" }
}
```

**Expected error:**
```json
{
  "success": false,
  "error": "Room Room 1 is already booked for the selected time slot"
}
```

---

## ?? Error Response Format

All errors follow this format:

```json
{
  "success": false,
  "error": "Error message here",
  "details": {
    "additionalInfo": "..."
  }
}
```

### HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized (invalid/missing token) |
| 403 | Forbidden (insufficient permissions) |
| 404 | Not Found |
| 409 | Conflict (booking conflict) |
| 500 | Internal Server Error |

---

## ?? Notes

### Date Format
Always use: `YYYY-MM-DD` (e.g., `2025-01-15`)

### Time Format
Always use: `HH:MM` in 24-hour format (e.g., `18:00`, `09:30`)

### Time Slots
- Operating hours: 09:00 - 23:00
- Intervals: 30 minutes
- Valid times: 09:00, 09:30, 10:00, ..., 22:30, 23:00

### Phone Format
Accepts various formats:
- `01234567890`
- `+201234567890`
- `0123 456 7890`

---

## ?? Environment

### Development
```
Base URL: http://localhost:5000
Swagger: http://localhost:5000/swagger
```

### Production (when deployed)
```
Base URL: https://your-domain.com
```

---

## ?? Import to Postman

1. Copy any request
2. In Postman: Import ? Raw Text ? Paste
3. Set environment variable `baseUrl` = `http://localhost:5000`
4. Set environment variable `token` after login

---

**For interactive testing, use Swagger UI: http://localhost:5000/swagger**
