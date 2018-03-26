USE WebCalendar;

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AspNetRoleClaims]'))
BEGIN
	CREATE TABLE [dbo].[AspNetRoleClaims] (
		[Id]         INT            IDENTITY (1, 1) NOT NULL,
		[ClaimType]  NVARCHAR (MAX) NULL,
		[ClaimValue] NVARCHAR (MAX) NULL,
		[RoleId]     INT            NOT NULL,
		CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
	);


	CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
		ON [dbo].[AspNetRoleClaims]([RoleId] ASC);
END
