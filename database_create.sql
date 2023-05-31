 USE [master]
GO
/****** Object:  Database [EquipmentAccounting]    Script Date: 30.05.2023 21:02:20 ******/
CREATE DATABASE [EquipmentAccounting]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EquipmentAccounting', FILENAME = N'D:\Program Files (x86)\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EquipmentAccounting.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EquipmentAccounting_log', FILENAME = N'D:\Program Files (x86)\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EquipmentAccounting_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [EquipmentAccounting] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EquipmentAccounting].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EquipmentAccounting] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET ARITHABORT OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EquipmentAccounting] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EquipmentAccounting] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EquipmentAccounting] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EquipmentAccounting] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET RECOVERY FULL 
GO
ALTER DATABASE [EquipmentAccounting] SET  MULTI_USER 
GO
ALTER DATABASE [EquipmentAccounting] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EquipmentAccounting] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EquipmentAccounting] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EquipmentAccounting] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EquipmentAccounting] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EquipmentAccounting] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EquipmentAccounting', N'ON'
GO
ALTER DATABASE [EquipmentAccounting] SET QUERY_STORE = OFF
GO
USE [EquipmentAccounting]
GO
/****** Object:  Table [dbo].[Deliveries]    Script Date: 30.05.2023 21:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deliveries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[EquipmentID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
 CONSTRAINT [PK__Deliveri__3214EC27B40C5F15] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Distributions]    Script Date: 30.05.2023 21:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Distributions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentID] [int] NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[UserID] [int] NOT NULL,
	[EquipmentCount] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK__Distribu__3214EC27B65C9795] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipments]    Script Date: 30.05.2023 21:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[CountInStock] [int] NOT NULL,
 CONSTRAINT [PK__Equipmen__3214EC27450D8692] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 30.05.2023 21:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 30.05.2023 21:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[Contact] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 30.05.2023 21:02:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK__Users__3214EC27996597F0] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Deliveries]  WITH CHECK ADD  CONSTRAINT [FK_Deliveries_Equipments] FOREIGN KEY([EquipmentID])
REFERENCES [dbo].[Equipments] ([ID])
GO
ALTER TABLE [dbo].[Deliveries] CHECK CONSTRAINT [FK_Deliveries_Equipments]
GO
ALTER TABLE [dbo].[Deliveries]  WITH CHECK ADD  CONSTRAINT [FK_Deliveries_Suppliers] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([ID])
GO
ALTER TABLE [dbo].[Deliveries] CHECK CONSTRAINT [FK_Deliveries_Suppliers]
GO
ALTER TABLE [dbo].[Distributions]  WITH CHECK ADD  CONSTRAINT [FK_Distributions_Equipments] FOREIGN KEY([EquipmentID])
REFERENCES [dbo].[Equipments] ([ID])
GO
ALTER TABLE [dbo].[Distributions] CHECK CONSTRAINT [FK_Distributions_Equipments]
GO
ALTER TABLE [dbo].[Distributions]  WITH CHECK ADD  CONSTRAINT [FK_Distributions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Distributions] CHECK CONSTRAINT [FK_Distributions_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
USE [master]
GO
ALTER DATABASE [EquipmentAccounting] SET  READ_WRITE 
GO
