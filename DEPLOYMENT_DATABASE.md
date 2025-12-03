# Render.com PostgreSQL Setup

## Steps to Create Free PostgreSQL Database

1. **Sign Up**
   - Go to https://render.com
   - Sign up with GitHub (recommended)

2. **Create PostgreSQL Database**
   - Click "New +"
   - Select "PostgreSQL"
   - Fill in details:
 * **Name**: playzone-db
   * **Database**: playzone_db
     * **User**: playzoneadmin
     * **Region**: Choose closest region
     * **PostgreSQL Version**: 16
     * **Datadog API Key**: Leave empty
     * **Plans**: Select "Free"

3. **Get Connection Details**
   - Internal Database URL: Copy this
   - External Database URL: Use this for external connections

4. **Format for .NET Connection String**
   ```
   Host=your-host.render.com;Port=5432;Database=playzone_db;Username=playzoneadmin;Password=YOUR_PASSWORD;SSL Mode=Require
   ```

## Important Notes
- Free tier: Database spins down after 15 mins of inactivity
- No backup data after 3 months of inactivity
- Recommended: Upgrade to $7/month for production

## PostgreSQL Initial Setup
After creation, seed your database:

```sql
-- Connect to database
\c playzone_db

-- Tables will be created by EF Core migrations
-- Initial data (8 rooms, admin user) seeded on first run
```
