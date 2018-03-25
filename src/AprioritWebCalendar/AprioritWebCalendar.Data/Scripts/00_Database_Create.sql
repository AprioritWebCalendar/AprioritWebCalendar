IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'WebCalendar')
BEGIN
    CREATE DATABASE WebCalendar
END;