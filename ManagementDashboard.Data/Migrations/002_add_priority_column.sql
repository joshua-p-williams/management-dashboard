-- 002_add_priority_column.sql
-- Adds the Priority column to the Tasks table for Eisenhower Matrix

ALTER TABLE Tasks ADD COLUMN Priority INTEGER NOT NULL DEFAULT 0; -- 0 = Low, 1 = Medium, 2 = High
