# PlayZone Backend - Setup Guide

## Prerequisites

- .NET 8 SDK
- Docker Desktop
- PostgreSQL (via Docker)
- Visual Studio 2022 or VS Code

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/mhmdalaa-sim/brave
cd brave/Backend/PlayZone
```

### 2. Setup Environment Variables

Copy the example environment file and configure it:

```bash
cp .env.example .env
```

Edit `.env` file with your configuration.

### 3. Start Docker Containers

Start PostgreSQL and pgAdmin using Docker Compose:

```bash
docker-compose up -d
```

This will start:
- **PostgreSQL** on port `5432`
- **pgAdmin** on port `5050`

### 4. Verify Docker Containers

Check if containers are running:

```bash
docker ps
```

You should see `playzone_postgres` and `playzone_pgadmin` running.

### 5. Access pgAdmin

Open your browser and navigate to:
```
http://localhost:5050
```

Login with:
- **Email**: admin@playzone.com
- **Password**: admin123 (or your configured password)

### 6. Connect to PostgreSQL in pgAdmin

1. Click "Add New Server"
2. **General Tab**:
 - Name: PlayZone DB
3. **Connection Tab**:
   - Host: postgres (or localhost if connecting from host machine)
   - Port: 5432
   - Database: playzone_db
   - Username: playzone
   - Password: playzone123

### 7. Restore NuGet Packages

```bash
dotnet restore
```

### 8. Run Database Migrations

The application will automatically run migrations on startup, but you can also run them manually:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 9. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`

## Docker Commands

### Start containers
```bash
docker-compose up -d
```

### Stop containers
```bash
docker-compose down
```

### View logs
```bash
docker-compose logs -f
```

### Restart containers
```bash
docker-compose restart
```

### Remove containers and volumes (?? WARNING: This will delete all data)
```bash
docker-compose down -v
```

## Database Connection

### From Application (localhost)
```
Host=localhost;Port=5432;Database=playzone_db;Username=playzone;Password=playzone123
```

### From Docker Container
```
Host=postgres;Port=5432;Database=playzone_db;Username=playzone;Password=playzone123
```

## Project Structure

```
PlayZone/
??? Configuration/       # Configuration classes
??? Controllers/  # API Controllers
??? Data/ # DbContext and migrations
??? Models/         # Entity models
??? Services/              # Business logic
??? DTOs/          # Data Transfer Objects
??? Repositories/       # Data access layer
??? Middleware/         # Custom middleware
??? Utilities/             # Helper utilities
??? docker-compose.yml     # Docker configuration
??? .env            # Environment variables (not in git)
??? .env.example         # Environment template
??? appsettings.json       # Application settings
??? Program.cs             # Application entry point
```

## Default Credentials

### Database
- Username: `playzone`
- Password: `playzone123`
- Database: `playzone_db`

### pgAdmin
- Email: `admin@playzone.com`
- Password: `admin123`

### Admin User (Application)
- Username: `admin`
- Password: `Admin@123`

**?? Important**: Change these default passwords in production!

## Troubleshooting

### Port Already in Use

If ports 5432 or 5050 are already in use, edit `.env` file:

```env
POSTGRES_PORT=5433
PGADMIN_PORT=5051
```

Then update `appsettings.json` connection string accordingly.

### Database Connection Failed

1. Ensure Docker containers are running: `docker ps`
2. Check container logs: `docker-compose logs postgres`
3. Verify connection string in `appsettings.json`

### Migrations Not Running

Run migrations manually:

```bash
dotnet ef database update
```

If migrations don't exist:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Seeded Data

The database is automatically seeded with:
- **8 Rooms** (Room 1 through Room 8)
- **1 Admin User** (username: admin, password: Admin@123)
- **WhatsApp Configuration** (default number: +201234567890)

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `POSTGRES_USER` | PostgreSQL username | playzone |
| `POSTGRES_PASSWORD` | PostgreSQL password | playzone123 |
| `POSTGRES_DB` | Database name | playzone_db |
| `POSTGRES_PORT` | PostgreSQL port | 5432 |
| `PGADMIN_EMAIL` | pgAdmin email | admin@playzone.com |
| `PGADMIN_PASSWORD` | pgAdmin password | admin123 |
| `PGADMIN_PORT` | pgAdmin port | 5050 |
| `DATABASE_URL` | Full database connection string | - |
| `JWT_SECRET` | JWT signing secret | - |
| `ADMIN_PASSWORD` | Default admin password | Admin@123 |

## Next Steps

1. ? Docker setup complete
2. ? Database configured
3. ? Entity models created
4. ? Implement repositories
5. ? Implement services
6. ? Implement controllers
7. ? Add validation
8. ? Add unit tests

## Support

For issues or questions, please create an issue on GitHub.
