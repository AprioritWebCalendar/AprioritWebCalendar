﻿USE WebCalendar

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AspNetUsers]'))
BEGIN
    CREATE TABLE [dbo].[AspNetUsers] (
        [Id]                   INT                IDENTITY (1, 1) NOT NULL,
        [AccessFailedCount]    INT                NOT NULL,
        [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
        [Email]                NVARCHAR (256)     NULL,
        [EmailConfirmed]       BIT                NOT NULL,
        [LockoutEnabled]       BIT                NOT NULL,
        [LockoutEnd]           DATETIMEOFFSET (7) NULL,
        [NormalizedEmail]      NVARCHAR (256)     NULL,
        [NormalizedUserName]   NVARCHAR (256)     NULL,
        [PasswordHash]         NVARCHAR (MAX)     NULL,
        [PhoneNumber]          NVARCHAR (MAX)     NULL,
        [PhoneNumberConfirmed] BIT                NOT NULL,
        [SecurityStamp]        NVARCHAR (MAX)     NULL,
        [TwoFactorEnabled]     BIT                NOT NULL,
        [UserName]             NVARCHAR (256)     NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE NONCLUSTERED INDEX [EmailIndex]
        ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);

    CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
        ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);
END
