
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/24/2019 00:46:47
-- Generated from EDMX file: C:\Users\hp\Desktop\PNMS\src\PNMS.Web.API\DataLayer\Entities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_NewsCategoryItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NewsItems] DROP CONSTRAINT [FK_NewsCategoryItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[NewsCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsCategories];
GO
IF OBJECT_ID(N'[dbo].[NewsItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NewsItems];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'NewsCategories'
CREATE TABLE [dbo].[NewsCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Image] nvarchar(max)  NULL
);
GO

-- Creating table 'NewsItems'
CREATE TABLE [dbo].[NewsItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Text] nvarchar(max)  NULL,
    [Date] datetime  NULL,
    [LinkUrl] nvarchar(max)  NULL,
    [NewsCategoryId] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'NewsCategories'
ALTER TABLE [dbo].[NewsCategories]
ADD CONSTRAINT [PK_NewsCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NewsItems'
ALTER TABLE [dbo].[NewsItems]
ADD CONSTRAINT [PK_NewsItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [NewsCategoryId] in table 'NewsItems'
ALTER TABLE [dbo].[NewsItems]
ADD CONSTRAINT [FK_NewsCategoryItem]
    FOREIGN KEY ([NewsCategoryId])
    REFERENCES [dbo].[NewsCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NewsCategoryItem'
CREATE INDEX [IX_FK_NewsCategoryItem]
ON [dbo].[NewsItems]
    ([NewsCategoryId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------