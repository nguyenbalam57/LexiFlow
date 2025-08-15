-- LexiFlow Database Creation Script
-- Execute this script in SQL Server Management Studio or sqlcmd

USE master;
GO

-- Create LexiFlow database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LexiFlow')
BEGIN
    CREATE DATABASE [LexiFlow]
    COLLATE SQL_Latin1_General_CP1_CI_AS;
    
    ALTER DATABASE [LexiFlow] SET COMPATIBILITY_LEVEL = 160;
    ALTER DATABASE [LexiFlow] SET ANSI_NULL_DEFAULT OFF;
    ALTER DATABASE [LexiFlow] SET ANSI_NULLS OFF;
    ALTER DATABASE [LexiFlow] SET ANSI_PADDING OFF;
    ALTER DATABASE [LexiFlow] SET ANSI_WARNINGS OFF;
    ALTER DATABASE [LexiFlow] SET ARITHABORT OFF;
    ALTER DATABASE [LexiFlow] SET AUTO_CLOSE OFF;
    ALTER DATABASE [LexiFlow] SET AUTO_SHRINK OFF;
    ALTER DATABASE [LexiFlow] SET AUTO_UPDATE_STATISTICS ON;
    ALTER DATABASE [LexiFlow] SET CURSOR_CLOSE_ON_COMMIT OFF;
    ALTER DATABASE [LexiFlow] SET CURSOR_DEFAULT GLOBAL;
    ALTER DATABASE [LexiFlow] SET CONCAT_NULL_YIELDS_NULL OFF;
    ALTER DATABASE [LexiFlow] SET NUMERIC_ROUNDABORT OFF;
    ALTER DATABASE [LexiFlow] SET QUOTED_IDENTIFIER OFF;
    ALTER DATABASE [LexiFlow] SET RECURSIVE_TRIGGERS OFF;
    ALTER DATABASE [LexiFlow] SET DISABLE_BROKER;
    ALTER DATABASE [LexiFlow] SET AUTO_UPDATE_STATISTICS_ASYNC OFF;
    ALTER DATABASE [LexiFlow] SET DATE_CORRELATION_OPTIMIZATION OFF;
    ALTER DATABASE [LexiFlow] SET TRUSTWORTHY OFF;
    ALTER DATABASE [LexiFlow] SET ALLOW_SNAPSHOT_ISOLATION OFF;
    ALTER DATABASE [LexiFlow] SET PARAMETERIZATION SIMPLE;
    ALTER DATABASE [LexiFlow] SET READ_COMMITTED_SNAPSHOT OFF;
    ALTER DATABASE [LexiFlow] SET HONOR_BROKER_PRIORITY OFF;
    ALTER DATABASE [LexiFlow] SET RECOVERY SIMPLE;
    ALTER DATABASE [LexiFlow] SET MULTI_USER;
    ALTER DATABASE [LexiFlow] SET PAGE_VERIFY CHECKSUM;
    ALTER DATABASE [LexiFlow] SET DB_CHAINING OFF;
    
    PRINT 'LexiFlow database created successfully!';
END
ELSE
BEGIN
    PRINT 'LexiFlow database already exists.';
END
GO

-- Switch to LexiFlow database
USE [LexiFlow];
GO

-- Create a test table to verify the database is working
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DatabaseTest' AND xtype='U')
BEGIN
    CREATE TABLE DatabaseTest (
        Id int IDENTITY(1,1) PRIMARY KEY,
        TestMessage nvarchar(255) NOT NULL,
        CreatedAt datetime2 DEFAULT GETUTCDATE()
    );
    
    INSERT INTO DatabaseTest (TestMessage) VALUES ('LexiFlow database is ready for Entity Framework migrations!');
    PRINT 'Test table created and test data inserted.';
END
GO

PRINT 'Database setup completed. You can now run Entity Framework migrations.';