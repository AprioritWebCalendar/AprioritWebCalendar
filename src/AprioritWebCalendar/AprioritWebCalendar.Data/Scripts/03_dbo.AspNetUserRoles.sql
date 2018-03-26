﻿USE WebCalendar;

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AspNetUserRoles]'))
BEGIN
	CREATE TABLE [dbo].[AspNetUserRoles] (
		[UserId] INT NOT NULL,
		[RoleId] INT NOT NULL,
		CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
		CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
		CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
	);

	CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
		ON [dbo].[AspNetUserRoles]([RoleId] ASC);
END

