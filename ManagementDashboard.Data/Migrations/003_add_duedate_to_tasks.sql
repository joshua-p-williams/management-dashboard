-- Migration: Add DueDate column to Tasks table (nullable)
ALTER TABLE Tasks ADD COLUMN DueDate DATETIME NULL;
