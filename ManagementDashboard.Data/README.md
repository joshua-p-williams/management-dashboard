# Data Layer for ManagementDashboard

This project contains the SQLite data access layer, migrations, and repository implementations for the Management Dashboard Platform.

## Structure

- `/Models` — Database models (e.g., EisenhowerTask)
- `/Migrations` — SQL migration scripts (e.g., 001_init.sql)
- `SqliteConnectionFactory` — Cross-platform SQLite connection provider
- Repositories (e.g., EisenhowerTaskRepository)

## Adding Migrations

1. Add a new `.sql` file to `/Migrations` (e.g., `002_add_column.sql`).
2. The migration runner will apply it automatically if not already applied.

## Updating Models/Repositories

- Update the model in `/Models`.
- Update or add repository interfaces/implementations as needed.

## Running Tests

- Use the in-memory SQLite connection string: `Data Source=:memory:`
- Mock or use a test `IConnectionFactory` for isolation.

## Coding Style

- Use PascalCase for class and property names.
- Keep models as POCOs with minimal logic.
- Use async methods for all DB operations.

---

For more, see the main solution README and docs/tasks.md.
