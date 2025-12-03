# ?? PlayZone - PlayStation Café Booking System

Full-stack application for managing PlayStation gaming room bookings with admin dashboard and WhatsApp integration.

## ?? Repository Structure

```
brave/
??? Backend/             # ASP.NET Core 8 API
?   ??? PlayZone/
?       ??? Program.cs
?       ??? appsettings.json
?       ??? PlayZone.csproj
?       ??? ...
?
??? Frontend/              # React App
?   ??? src/
?   ??? public/
?   ??? package.json
?   ??? ...
?
??? README.md
```

## ?? Quick Start

### Backend Setup
```sh
cd Backend/PlayZone
dotnet run
```
API runs on: `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

### Frontend Setup
```sh
cd Frontend
npm install
npm start
```
App runs on: `http://localhost:3000`

## ?? Tech Stack

### Backend
- ASP.NET Core 8
- PostgreSQL
- Entity Framework Core
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- React 18
- Vite/Create React App
- Axios
- TailwindCSS (or your CSS framework)

## ?? Default Credentials

**Admin Login:**
- Username: `admin`
- Password: `Admin@123`

## ?? Documentation

- Backend: See `Backend/PlayZone/README.md`
- Frontend: See `Frontend/README.md`

## ?? Deployment

- Backend: Deployed on Vercel (free tier)
- Frontend: Deployed on Vercel or Netlify (free tier)
- Database: Azure PostgreSQL or Render.com (free tier)

## ?? Support

For issues or questions, check the documentation or create an issue.
