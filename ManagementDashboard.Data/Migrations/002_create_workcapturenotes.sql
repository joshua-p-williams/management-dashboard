-- Migration: Create WorkCaptureNotes table for scrum summary feature
CREATE TABLE IF NOT EXISTS WorkCaptureNotes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Notes TEXT,
    TaskId INTEGER,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id)
);
