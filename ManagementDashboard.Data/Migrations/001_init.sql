CREATE TABLE IF NOT EXISTS _Migrations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MigrationName TEXT NOT NULL UNIQUE,
    AppliedOnUtc DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS Tasks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT,
    Quadrant TEXT,
    Priority INTEGER NOT NULL DEFAULT 0,
    CompletedAt DATETIME,
    BlockerReason TEXT,
    BlockedAt DATETIME,
    UnblockedAt DATETIME,
    DelegatedTo TEXT,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    DeletedAt DATETIME
);
