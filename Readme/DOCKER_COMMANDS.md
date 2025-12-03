# ?? Docker Commands Cheat Sheet - PlayZone

## ?? Starting Services

### Start all containers (detached mode)
```bash
docker-compose up -d
```

### Start and view logs
```bash
docker-compose up
```

### Start specific service
```bash
docker-compose up -d postgres
docker-compose up -d pgadmin
```

---

## ?? Stopping Services

### Stop all containers
```bash
docker-compose down
```

### Stop but keep volumes (data persists)
```bash
docker-compose stop
```

### Stop and remove volumes (?? DELETES ALL DATA)
```bash
docker-compose down -v
```

---

## ?? Monitoring

### View running containers
```bash
docker-compose ps
```

or
```bash
docker ps
```

### View logs (all services)
```bash
docker-compose logs
```

### View logs (specific service)
```bash
docker-compose logs postgres
docker-compose logs pgadmin
```

### Follow logs (live stream)
```bash
docker-compose logs -f postgres
```

### View last 50 lines
```bash
docker-compose logs --tail=50 postgres
```

---

## ?? Restarting Services

### Restart all
```bash
docker-compose restart
```

### Restart specific service
```bash
docker-compose restart postgres
```

---

## ??? Cleanup

### Remove stopped containers
```bash
docker-compose rm
```

### Remove everything (containers, networks, volumes)
```bash
docker-compose down -v --remove-orphans
```

### Remove all Docker images (frees disk space)
```bash
docker system prune -a
```

---

## ?? Volume Management

### List volumes
```bash
docker volume ls
```

### Inspect volume
```bash
docker volume inspect playzone_postgres_data
```

### Remove specific volume (?? DELETES DATA)
```bash
docker volume rm playzone_postgres_data
```

---

## ?? Debugging

### Execute command in running container
```bash
docker-compose exec postgres psql -U playzone -d playzone_db
```

### Access PostgreSQL CLI
```bash
docker exec -it playzone_postgres psql -U playzone -d playzone_db
```

### Access container shell
```bash
docker exec -it playzone_postgres sh
```

### Check container health
```bash
docker inspect playzone_postgres | grep Health
```

---

## ?? Common Issues & Solutions

### Port 5432 Already in Use

**Solution 1: Find and kill process**
```bash
# Windows
netstat -ano | findstr :5432
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :5432
kill -9 <PID>
```

**Solution 2: Change port in `.env`**
```env
POSTGRES_PORT=5433
```

Then update connection string:
```
Host=localhost;Port=5433;Database=playzone_db;...
```

### PostgreSQL Not Starting

**Check logs:**
```bash
docker-compose logs postgres
```

**Common causes:**
- Port conflict
- Permission issues
- Corrupted volume

**Solution:**
```bash
docker-compose down -v
docker-compose up -d
```

### pgAdmin Can't Connect to Database

**From pgAdmin container:**
- Host: `postgres` (not localhost)
- Port: `5432`

**From host machine:**
- Host: `localhost`
- Port: `5432`

### Database Data Lost After Restart

**Check if volumes are persistent:**
```bash
docker volume ls
```

Should see:
- `playzone_postgres_data`
- `playzone_pgadmin_data`

### Out of Disk Space

**Clean up:**
```bash
docker system prune -a --volumes
```

### Containers Keep Restarting

**Check logs:**
```bash
docker-compose logs --tail=100
```

**Common fixes:**
- Restart Docker Desktop
- Remove and recreate containers
- Check firewall/antivirus

---

## ?? Useful SQL Commands

### Connect to database
```bash
docker exec -it playzone_postgres psql -U playzone -d playzone_db
```

### List tables
```sql
\dt
```

### Describe table
```sql
\d rooms
\d bookings
```

### Count records
```sql
SELECT COUNT(*) FROM rooms;
SELECT COUNT(*) FROM bookings;
```

### View all bookings
```sql
SELECT * FROM bookings;
```

### Exit PostgreSQL CLI
```sql
\q
```

---

## ?? Complete Reset (Fresh Start)

```bash
# 1. Stop all containers
docker-compose down -v

# 2. Remove all PlayZone volumes
docker volume rm playzone_postgres_data
docker volume rm playzone_pgadmin_data

# 3. Remove migrations (optional)
rm -rf Migrations

# 4. Recreate migration
dotnet ef migrations add InitialCreate

# 5. Start containers
docker-compose up -d

# 6. Run application (migrations auto-apply)
dotnet run
```

---

## ?? Network Commands

### List networks
```bash
docker network ls
```

### Inspect network
```bash
docker network inspect playzone_playzone_network
```

### Test connectivity
```bash
docker exec playzone_postgres ping pgadmin
```

---

## ?? Backup & Restore

### Backup database
```bash
docker exec playzone_postgres pg_dump -U playzone playzone_db > backup.sql
```

### Restore database
```bash
docker exec -i playzone_postgres psql -U playzone playzone_db < backup.sql
```

### Export to CSV
```bash
docker exec playzone_postgres psql -U playzone -d playzone_db \
  -c "COPY bookings TO STDOUT WITH CSV HEADER" > bookings.csv
```

---

## ?? Performance Monitoring

### Check resource usage
```bash
docker stats
```

### Check specific container
```bash
docker stats playzone_postgres
```

### View container details
```bash
docker inspect playzone_postgres
```

---

## ?? Quick Reference

| Task | Command |
|------|---------|
| Start | `docker-compose up -d` |
| Stop | `docker-compose down` |
| Restart | `docker-compose restart` |
| Logs | `docker-compose logs -f` |
| Status | `docker-compose ps` |
| Clean | `docker-compose down -v` |
| Shell | `docker exec -it playzone_postgres sh` |
| SQL | `docker exec -it playzone_postgres psql -U playzone -d playzone_db` |

---

## ?? Emergency Reset

```bash
# Nuclear option - removes everything
docker stop $(docker ps -aq)
docker rm $(docker ps -aq)
docker volume rm $(docker volume ls -q)
docker network prune -f
docker system prune -a --volumes -f
```

**Then start fresh:**
```bash
docker-compose up -d
```

---

**For more help, check Docker documentation: https://docs.docker.com/**
