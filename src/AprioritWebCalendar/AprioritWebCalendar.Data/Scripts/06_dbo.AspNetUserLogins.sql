USE WebCalendar;

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AspNetUserLogins]'))
BEGIN
	CREATE TABLE [dbo].[AspNetUserLogins] (
		[LoginProvider]       NVARCHAR (450) NOT NULL,
		[ProviderKey]         NVARCHAR (450) NOT NULL,
		[ProviderDisplayName] NVARCHAR (MAX) NULL,
		[UserId]              INT            NOT NULL,
		CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
		CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
	);


	CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
		ON [dbo].[AspNetUserLogins]([UserId] ASC);

END
