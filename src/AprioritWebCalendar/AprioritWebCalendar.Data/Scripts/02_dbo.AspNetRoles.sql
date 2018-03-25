USE WebCalendar;

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AspNetRoles]'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoles] (
        [Id]               INT            IDENTITY (1, 1) NOT NULL,
        [ConcurrencyStamp] NVARCHAR (MAX) NULL,
        [Name]             NVARCHAR (256) NULL,
        [NormalizedName]   NVARCHAR (256) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
        ON [dbo].[AspNetRoles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);

END
