USE [master]
GO
/****** Object:  Database [personnelDB]    Script Date: 2021/6/10 21:15:14 ******/
CREATE DATABASE [personnelDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'personnelDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\personnelDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'personnelDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\personnelDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [personnelDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [personnelDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [personnelDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [personnelDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [personnelDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [personnelDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [personnelDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [personnelDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [personnelDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [personnelDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [personnelDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [personnelDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [personnelDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [personnelDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [personnelDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [personnelDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [personnelDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [personnelDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [personnelDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [personnelDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [personnelDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [personnelDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [personnelDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [personnelDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [personnelDB] SET RECOVERY FULL 
GO
ALTER DATABASE [personnelDB] SET  MULTI_USER 
GO
ALTER DATABASE [personnelDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [personnelDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [personnelDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [personnelDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [personnelDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [personnelDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'personnelDB', N'ON'
GO
ALTER DATABASE [personnelDB] SET QUERY_STORE = OFF
GO
USE [personnelDB]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceId] [int] IDENTITY(20210000,1) NOT NULL,
	[StaffId] [int] NOT NULL,
	[StartTime] [smalldatetime] NULL,
	[EndTime] [smalldatetime] NULL,
	[WorkStatus] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepartManager]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepartManager](
	[Id] [int] IDENTITY(20210000,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[StaffId] [int] NOT NULL,
	[Comment] [varchar](200) NULL,
 CONSTRAINT [PK__DepartMa__3214EC0746DDE30F] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentId] [int] IDENTITY(20210000,1) NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[OfficeLocal] [varchar](50) NULL,
	[CreateTime] [datetime] NOT NULL,
	[Authority] [int] NOT NULL,
 CONSTRAINT [PK__Departme__B2079BED01FF2DBC] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OpLog]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpLog](
	[OpId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Content] [varchar](100) NULL,
	[CreateTime] [smalldatetime] NOT NULL,
	[StaffId] [int] NOT NULL,
 CONSTRAINT [PK__OpLog__46E40E08E348498B] PRIMARY KEY CLUSTERED 
(
	[OpId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Salary]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Salary](
	[SalaryId] [int] IDENTITY(20210000,1) NOT NULL,
	[OverTime] [int] NULL,
	[PartWage1] [smallmoney] NULL,
	[PartWage2] [smallmoney] NULL,
	[PartWage3] [smallmoney] NULL,
	[CreateTime] [smalldatetime] NOT NULL,
	[RealWage] [smallmoney] NULL,
	[StaffId] [int] NOT NULL,
 CONSTRAINT [PK__Salary__4BE2045713274DCD] PRIMARY KEY CLUSTERED 
(
	[SalaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffId] [int] IDENTITY(20210000,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Age] [int] NOT NULL,
	[Sex] [nvarchar](10) NOT NULL,
	[Post] [int] NOT NULL,
	[Address] [varchar](20) NULL,
	[Phone] [varchar](11) NULL,
	[Enable] [bit] NOT NULL,
	[EntryTime] [date] NOT NULL,
	[LeaveTime] [date] NULL,
	[PassWord] [varchar](20) NOT NULL,
 CONSTRAINT [PK__Staff__96D4AB1779AEA653] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Staff]    Script Date: 2021/6/10 21:15:14 ******/
CREATE NONCLUSTERED INDEX [IX_Staff] ON [dbo].[Staff]
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT (getdate()) FOR [StartTime]
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT ((0)) FOR [WorkStatus]
GO
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF__Departmen__Offic__2D27B809]  DEFAULT ('未填写') FOR [OfficeLocal]
GO
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_Authority]  DEFAULT ((5)) FOR [Authority]
GO
ALTER TABLE [dbo].[OpLog] ADD  CONSTRAINT [DF__OpLog__CreateTim__5629CD9C]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Salary] ADD  CONSTRAINT [DF__Salary__OverTime__45F365D3]  DEFAULT ((0)) FOR [OverTime]
GO
ALTER TABLE [dbo].[Salary] ADD  CONSTRAINT [DF__Salary__PartWage__46E78A0C]  DEFAULT (NULL) FOR [PartWage1]
GO
ALTER TABLE [dbo].[Salary] ADD  CONSTRAINT [DF__Salary__partWage__47DBAE45]  DEFAULT (NULL) FOR [PartWage2]
GO
ALTER TABLE [dbo].[Salary] ADD  CONSTRAINT [DF__Salary__partWage__48CFD27E]  DEFAULT (NULL) FOR [PartWage3]
GO
ALTER TABLE [dbo].[Salary] ADD  CONSTRAINT [DF__Salary__CreateTi__49C3F6B7]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF__Staff__Post__32E0915F]  DEFAULT ((10000)) FOR [Post]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_Enable]  DEFAULT ((1)) FOR [Enable]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_EntryTime]  DEFAULT (getdate()) FOR [EntryTime]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_PassWord]  DEFAULT ((123456)) FOR [PassWord]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Staff]
GO
ALTER TABLE [dbo].[OpLog]  WITH CHECK ADD  CONSTRAINT [FK_OpLog_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[OpLog] CHECK CONSTRAINT [FK_OpLog_Staff]
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD  CONSTRAINT [FK_Salary_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Salary] CHECK CONSTRAINT [FK_Salary_Staff]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Department]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [CK__Staff__Sex__31EC6D26] CHECK  (([Sex]='男' OR [Sex]='女' OR [Sex]='其他'))
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [CK__Staff__Sex__31EC6D26]
GO
/****** Object:  StoredProcedure [dbo].[clock]    Script Date: 2021/6/10 21:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  proc [dbo].[clock]
@Time smalldatetime,
@StaffId int
as
begin
	DECLARE @current smalldatetime = getdate();  
	DECLARE @date date = @current;
	DECLARE @aid int;
	IF NOT EXISTS (select AttendanceId from Attendance where StaffId  = @StaffId and cast(StartTime as date) = @date)
	BEGIN
		insert into Attendance (StaffId) values(@StaffId)
	END	
	
	UPDATE Attendance set WorkStatus = (
	case 
	when @current<@Time and (WorkStatus&4 = 0) then WorkStatus | 4
	when @current>@Time and (WorkStatus&4 = 0) then WorkStatus | 5
	when @current<@Time and (WorkStatus&4 = 4) then WorkStatus | 10
	when @current>@Time and (WorkStatus&4 = 4) then WorkStatus | 8 
	else WorkStatus
	end
	),
	EndTime = (
	case
	when WorkStatus&4 = 4 then getdate()
	else NULL
	end
	)
	where StaffId  = @StaffId and cast(StartTime as date) = @date

	

end
GO
USE [master]
GO
ALTER DATABASE [personnelDB] SET  READ_WRITE 
GO
