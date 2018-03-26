USE WebCalendar;

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AspNetUserClaims]'))
BEGIN
	CREATE TABLE [dbo].[AspNetUserClaims] (
		[Id]         INT            IDENTITY (1, 1) NOT NULL,
		[ClaimType]  NVARCHAR (MAX) NULL,
		[ClaimValue] NVARCHAR (MAX) NULL,
		[UserId]     INT            NOT NULL,
		CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
	);


	CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
		ON [dbo].[AspNetUserClaims]([UserId] ASC);
END

