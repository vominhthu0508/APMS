USE [MyFramework]
GO
/****** Object:  Table [dbo].[Role_Type]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role_Type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role_Type_Name] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Role_Type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Type]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Type_Name] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.User_Type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company_Type]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company_Type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Type_Name] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Company_Type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeePlan]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeePlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Type_Id] [int] NOT NULL,
	[FeePlan_Name] [nvarchar](max) NULL,
	[FeePlay_Type] [int] NOT NULL,
	[FeePlan_Price] [int] NOT NULL,
	[FeePlan_Count] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.FeePlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Course]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Type_Id] [int] NOT NULL,
	[Course_Name] [nvarchar](100) NOT NULL,
	[Course_Code] [nvarchar](100) NOT NULL,
	[Course_Semester_Count] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Type_Id] [int] NOT NULL,
	[Company_Name] [nvarchar](100) NOT NULL,
	[Company_Name_Abbrev] [nvarchar](100) NOT NULL,
	[Company_Logo] [nvarchar](512) NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Profile]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Profile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Profile_Name] [nvarchar](100) NOT NULL,
	[User_Profile_Phone] [nvarchar](20) NULL,
	[User_Profile_Email] [nvarchar](100) NOT NULL,
	[User_Type_Id] [int] NOT NULL,
	[Role_Type_Id] [int] NOT NULL,
	[User_Profile_Avatar] [nvarchar](100) NULL,
	[User_Profile_Facebook] [nvarchar](50) NULL,
	[User_Profile_Address] [nvarchar](1000) NULL,
	[User_Profile_Gender] [int] NULL,
	[User_Profile_Birthday] [date] NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.User_Profile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Id] [int] NOT NULL,
	[Resource_Name] [nvarchar](100) NOT NULL,
	[Resource_Capacity] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Resource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookOrder]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Id] [int] NOT NULL,
	[Indent_Date] [datetime] NOT NULL,
	[Indent_Number] [int] NOT NULL,
	[Center] [nvarchar](max) NULL,
	[Indent_Status] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.BookOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Username] [nvarchar](300) NOT NULL,
	[Account_Password] [nvarchar](300) NOT NULL,
	[Account_Name] [nvarchar](200) NOT NULL,
	[Account_Avatar] [nvarchar](100) NULL,
	[Account_Email] [nvarchar](100) NOT NULL,
	[HasSetPassword] [bit] NOT NULL,
	[User_Profile_Id] [int] NOT NULL,
	[Account_ActiveKey] [nvarchar](500) NULL,
	[Account_RecoverPasswordKey] [varchar](500) NULL,
	[Account_RecoverPasswordExpired] [datetime] NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Faculty]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Id] [int] NOT NULL,
	[FC_Name] [nvarchar](100) NOT NULL,
	[FC_Type] [int] NOT NULL,
	[FC_Gender] [int] NOT NULL,
	[FC_Phone] [nvarchar](max) NULL,
	[FC_Email] [nvarchar](max) NULL,
	[FC_Address] [nvarchar](max) NULL,
	[FC_Address_Company] [nvarchar](max) NULL,
	[FC_Birthday] [datetime] NOT NULL,
	[FC_Salary] [bigint] NOT NULL,
	[FC_WorkingHour] [int] NOT NULL,
	[FC_CMND] [nvarchar](max) NULL,
	[FC_CMND_NoiCap] [nvarchar](max) NULL,
	[FC_CMND_NgayCap] [datetime] NOT NULL,
	[FC_MST] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Faculty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseFamily]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseFamily](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Course_Id] [int] NOT NULL,
	[CourseFamily_Name] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[CourseFamily_Year] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CourseFamily] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeePlan_Detail]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeePlan_Detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FeePlan_Id] [int] NOT NULL,
	[FeePlan_Index] [int] NOT NULL,
	[FeePlan_Amount] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.FeePlan_Detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseFamily_Id] [int] NOT NULL,
	[Semester] [int] NOT NULL,
	[Module_Name] [nvarchar](100) NOT NULL,
	[Module_Code] [nvarchar](100) NOT NULL,
	[Module_Type] [int] NOT NULL,
	[Module_Max_LT] [int] NOT NULL,
	[Module_Max_TH] [int] NOT NULL,
	[Module_DurationByHour] [int] NOT NULL,
	[Module_DurationByDay] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Module] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Company_Id] [int] NOT NULL,
	[CourseFamily_Id] [int] NOT NULL,
	[Class_Name] [nvarchar](100) NOT NULL,
	[Class_Admission_Date] [datetime] NOT NULL,
	[Class_Completion_Date] [datetime] NULL,
	[Class_Graduation_Date] [datetime] NULL,
	[Class_Day] [int] NOT NULL,
	[Class_Hour_Start] [int] NOT NULL,
	[Class_Hour_End] [int] NOT NULL,
	[Class_Studying_Status] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Class] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseFamily_Id] [int] NOT NULL,
	[Class_Id] [int] NOT NULL,
	[Student_EnrollNumber] [nvarchar](max) NULL,
	[Student_FirstName] [nvarchar](max) NULL,
	[Student_LastName] [nvarchar](max) NULL,
	[Student_Gender] [int] NOT NULL,
	[Student_Birthday] [datetime] NOT NULL,
	[Student_Avatar] [nvarchar](max) NULL,
	[Student_MobilePhone] [nvarchar](max) NULL,
	[Student_HomePhone] [nvarchar](max) NULL,
	[Student_Email] [nvarchar](max) NULL,
	[Student_Facebook] [nvarchar](max) NULL,
	[Student_Address] [nvarchar](max) NULL,
	[Student_ContactAddress] [nvarchar](max) NULL,
	[Student_City] [nvarchar](max) NULL,
	[Student_District] [nvarchar](max) NULL,
	[Student_Sponsor] [nvarchar](max) NULL,
	[Student_Sponsor_Relation] [nvarchar](max) NULL,
	[Student_Sponsor_Address] [nvarchar](max) NULL,
	[Student_Application_Date] [datetime] NOT NULL,
	[Student_Application_CS] [nvarchar](max) NULL,
	[Student_Application_Documents] [nvarchar](max) NULL,
	[Student_Status] [int] NOT NULL,
	[Student_Status_Date] [datetime] NOT NULL,
	[Student_Promotion] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Student] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class_Module]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class_Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Class_Id] [int] NOT NULL,
	[Module_Id] [int] NOT NULL,
	[Faculty_Id] [int] NOT NULL,
	[Resource_LT_Id] [int] NOT NULL,
	[Resource_TH_Id] [int] NOT NULL,
	[Resource_Exam_Id] [int] NOT NULL,
	[Class_Module_Name] [nvarchar](max) NULL,
	[Class_Module_Date_Start] [datetime] NOT NULL,
	[Class_Module_Date_End] [datetime] NOT NULL,
	[Class_Module_Date_Exam] [datetime] NOT NULL,
	[Class_Module_Hour_Start] [int] NOT NULL,
	[Class_Module_Hour_End] [int] NOT NULL,
	[Class_Module_DurationByDay] [int] NOT NULL,
	[Class_Module_Note] [nvarchar](max) NULL,
	[Class_Module_RenderColor] [nvarchar](max) NULL,
	[Class_Module_Status] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Class_Module] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faculty_Module]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculty_Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Faculty_Id] [int] NOT NULL,
	[Module_Id] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Faculty_Module] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class_Module_Day]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class_Module_Day](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Class_Module_Id] [int] NOT NULL,
	[Class_Module_Day_STT] [int] NOT NULL,
	[Class_Module_Day_Date] [datetime] NOT NULL,
	[Class_Module_Day_Hour_Start] [int] NOT NULL,
	[Class_Module_Day_Hour_End] [int] NOT NULL,
	[Class_Module_Day_Status] [int] NOT NULL,
	[Class_Module_Day_Note] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Class_Module_Day] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookOrder_Detail]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookOrder_Detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[BookOrder_Id] [int] NOT NULL,
	[Semester] [int] NOT NULL,
	[BookCode] [nvarchar](max) NULL,
	[BookPrice] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.BookOrder_Detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class_Module_StudentExam]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class_Module_StudentExam](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[Class_Module_Id] [int] NOT NULL,
	[Student_Status] [int] NOT NULL,
	[Mark_LT] [int] NOT NULL,
	[Mark_TH] [int] NOT NULL,
	[Mark_LT_Percentage] [int] NOT NULL,
	[Mark_TH_Percentage] [int] NOT NULL,
	[Exam_Count] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Class_Module_StudentExam] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_FeePlan]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_FeePlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[FeePlan_Id] [int] NOT NULL,
	[Nominal_Course_Fee] [int] NOT NULL,
	[Discount_Amount] [int] NOT NULL,
	[Actual_Course_Fee] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Student_FeePlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_ClassHistory]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_ClassHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[Class_Id] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ChangeReason] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Student_ClassHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_AcademicStatus]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_AcademicStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[Student_Status] [int] NOT NULL,
	[Student_Status_Date] [datetime] NOT NULL,
	[Student_Status_Note] [nvarchar](max) NULL,
	[Student_FU_Date] [datetime] NULL,
	[Student_FU_Amount] [bigint] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Student_AcademicStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_FeePlan_Installment]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_FeePlan_Installment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_FeePlan_Id] [int] NOT NULL,
	[FeePlan_Detail_Id] [int] NOT NULL,
	[Date_Planning] [datetime] NOT NULL,
	[Date_Actual] [datetime] NOT NULL,
	[Date_Extend] [datetime] NOT NULL,
	[Extend_Count] [int] NOT NULL,
	[Actual_Amount] [int] NOT NULL,
	[Installment_Status] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Student_FeePlan_Installment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prize]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prize](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[Exam_Id] [int] NULL,
	[Title] [nvarchar](max) NULL,
	[Prize_Date] [datetime] NOT NULL,
	[Prize_Type] [int] NOT NULL,
	[Prize_Semester] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Prize] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class_Module_Day_Student]    Script Date: 07/24/2016 19:32:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class_Module_Day_Student](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Id] [int] NOT NULL,
	[Class_Module_Day_Id] [int] NOT NULL,
	[Attendance_Status] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Class_Module_Day_Student] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF__BookOrder__Statu__693CA210]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[BookOrder] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__BookOrder__Creat__6A30C649]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[BookOrder] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__BookOrder__Creat__6B24EA82]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[BookOrder_Detail] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Class__Status__6E01572D]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Class__Created_D__6EF57B66]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Class_Mod__Statu__6FE99F9F]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Class_Mod__Creat__70DDC3D8]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Class_Mod__Statu__71D1E811]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Class_Mod__Creat__72C60C4A]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Class_Mod__Statu__73BA3083]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day_Student] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Class_Mod__Creat__74AE54BC]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day_Student] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Class_Mod__Statu__75A278F5]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_StudentExam] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Class_Mod__Creat__76969D2E]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_StudentExam] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Company__Status__6754599E]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Company] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Company__Created__68487DD7]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Company] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Company_T__Statu__656C112C]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Company_Type] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Company_T__Creat__66603565]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Company_Type] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Course__Status__00200768]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Course] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Course__Created___01142BA1]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Course] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__CourseFam__Statu__7E37BEF6]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[CourseFamily] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__CourseFam__Creat__7F2BE32F]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[CourseFamily] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__CourseFam__Cours__0F624AF8]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[CourseFamily] ADD  DEFAULT ((0)) FOR [CourseFamily_Year]
GO
/****** Object:  Default [DF__Faculty__Status__797309D9]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Faculty] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Faculty__Created__7A672E12]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Faculty] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Faculty_M__Creat__7B5B524B]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Faculty_Module] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__FeePlan__Status__09A971A2]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[FeePlan] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__FeePlan__Created__0A9D95DB]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[FeePlan] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__FeePlan_D__Statu__0B91BA14]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[FeePlan_Detail] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__FeePlan_D__Creat__0C85DE4D]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[FeePlan_Detail] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Module__Status__7C4F7684]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Module] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Module__Created___7D439ABD]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Module] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Prize__Status__778AC167]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Prize] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Prize__Created_D__787EE5A0]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Prize] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Resource__Status__02084FDA]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Resource] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Resource__Create__02FC7413]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Resource] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Student__Status__6C190EBB]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Student__Created__6D0D32F4]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Student_A__Statu__05D8E0BE]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_AcademicStatus] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Student_A__Creat__06CD04F7]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_AcademicStatus] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Student_C__Statu__03F0984C]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_ClassHistory] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Student_C__Creat__04E4BC85]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_ClassHistory] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Student_F__Statu__07C12930]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Student_F__Creat__08B54D69]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  Default [DF__Student_F__Statu__0D7A0286]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan_Installment] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Student_F__Creat__0E6E26BF]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan_Installment] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Created_Date]
GO
/****** Object:  ForeignKey [FK_dbo.Account_dbo.User_Profile_User_Profile_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Account_dbo.User_Profile_User_Profile_Id] FOREIGN KEY([User_Profile_Id])
REFERENCES [dbo].[User_Profile] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_dbo.Account_dbo.User_Profile_User_Profile_Id]
GO
/****** Object:  ForeignKey [FK_dbo.BookOrder_dbo.Company_Company_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[BookOrder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookOrder_dbo.Company_Company_Id] FOREIGN KEY([Company_Id])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[BookOrder] CHECK CONSTRAINT [FK_dbo.BookOrder_dbo.Company_Company_Id]
GO
/****** Object:  ForeignKey [FK_dbo.BookOrder_Detail_dbo.BookOrder_BookOrder_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[BookOrder_Detail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookOrder_Detail_dbo.BookOrder_BookOrder_Id] FOREIGN KEY([BookOrder_Id])
REFERENCES [dbo].[BookOrder] ([Id])
GO
ALTER TABLE [dbo].[BookOrder_Detail] CHECK CONSTRAINT [FK_dbo.BookOrder_Detail_dbo.BookOrder_BookOrder_Id]
GO
/****** Object:  ForeignKey [FK_dbo.BookOrder_Detail_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[BookOrder_Detail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookOrder_Detail_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[BookOrder_Detail] CHECK CONSTRAINT [FK_dbo.BookOrder_Detail_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_dbo.Company_Company_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_dbo.Company_Company_Id] FOREIGN KEY([Company_Id])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_dbo.Class_dbo.Company_Company_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_dbo.CourseFamily_CourseFamily_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_dbo.CourseFamily_CourseFamily_Id] FOREIGN KEY([CourseFamily_Id])
REFERENCES [dbo].[CourseFamily] ([Id])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_dbo.Class_dbo.CourseFamily_CourseFamily_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_dbo.Class_Class_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_dbo.Class_Class_Id] FOREIGN KEY([Class_Id])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[Class_Module] CHECK CONSTRAINT [FK_dbo.Class_Module_dbo.Class_Class_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_dbo.Faculty_Faculty_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_dbo.Faculty_Faculty_Id] FOREIGN KEY([Faculty_Id])
REFERENCES [dbo].[Faculty] ([Id])
GO
ALTER TABLE [dbo].[Class_Module] CHECK CONSTRAINT [FK_dbo.Class_Module_dbo.Faculty_Faculty_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_dbo.Module_Module_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_dbo.Module_Module_Id] FOREIGN KEY([Module_Id])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[Class_Module] CHECK CONSTRAINT [FK_dbo.Class_Module_dbo.Module_Module_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_dbo.Resource_Resource_Exam_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_dbo.Resource_Resource_Exam_Id] FOREIGN KEY([Resource_Exam_Id])
REFERENCES [dbo].[Resource] ([Id])
GO
ALTER TABLE [dbo].[Class_Module] CHECK CONSTRAINT [FK_dbo.Class_Module_dbo.Resource_Resource_Exam_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_dbo.Resource_Resource_LT_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_dbo.Resource_Resource_LT_Id] FOREIGN KEY([Resource_LT_Id])
REFERENCES [dbo].[Resource] ([Id])
GO
ALTER TABLE [dbo].[Class_Module] CHECK CONSTRAINT [FK_dbo.Class_Module_dbo.Resource_Resource_LT_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_dbo.Resource_Resource_TH_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_dbo.Resource_Resource_TH_Id] FOREIGN KEY([Resource_TH_Id])
REFERENCES [dbo].[Resource] ([Id])
GO
ALTER TABLE [dbo].[Class_Module] CHECK CONSTRAINT [FK_dbo.Class_Module_dbo.Resource_Resource_TH_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_Day_dbo.Class_Module_Class_Module_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_Day_dbo.Class_Module_Class_Module_Id] FOREIGN KEY([Class_Module_Id])
REFERENCES [dbo].[Class_Module] ([Id])
GO
ALTER TABLE [dbo].[Class_Module_Day] CHECK CONSTRAINT [FK_dbo.Class_Module_Day_dbo.Class_Module_Class_Module_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_Day_Student_dbo.Class_Module_Day_Class_Module_Day_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day_Student]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_Day_Student_dbo.Class_Module_Day_Class_Module_Day_Id] FOREIGN KEY([Class_Module_Day_Id])
REFERENCES [dbo].[Class_Module_Day] ([Id])
GO
ALTER TABLE [dbo].[Class_Module_Day_Student] CHECK CONSTRAINT [FK_dbo.Class_Module_Day_Student_dbo.Class_Module_Day_Class_Module_Day_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_Day_Student_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_Day_Student]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_Day_Student_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Class_Module_Day_Student] CHECK CONSTRAINT [FK_dbo.Class_Module_Day_Student_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_StudentExam_dbo.Class_Module_Class_Module_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_StudentExam]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_StudentExam_dbo.Class_Module_Class_Module_Id] FOREIGN KEY([Class_Module_Id])
REFERENCES [dbo].[Class_Module] ([Id])
GO
ALTER TABLE [dbo].[Class_Module_StudentExam] CHECK CONSTRAINT [FK_dbo.Class_Module_StudentExam_dbo.Class_Module_Class_Module_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Class_Module_StudentExam_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Class_Module_StudentExam]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Class_Module_StudentExam_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Class_Module_StudentExam] CHECK CONSTRAINT [FK_dbo.Class_Module_StudentExam_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Company_dbo.Company_Type_Company_Type_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Company_dbo.Company_Type_Company_Type_Id] FOREIGN KEY([Company_Type_Id])
REFERENCES [dbo].[Company_Type] ([Id])
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_dbo.Company_dbo.Company_Type_Company_Type_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Course_dbo.Company_Type_Company_Type_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Course_dbo.Company_Type_Company_Type_Id] FOREIGN KEY([Company_Type_Id])
REFERENCES [dbo].[Company_Type] ([Id])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_dbo.Course_dbo.Company_Type_Company_Type_Id]
GO
/****** Object:  ForeignKey [FK_dbo.CourseFamily_dbo.Course_Course_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[CourseFamily]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CourseFamily_dbo.Course_Course_Id] FOREIGN KEY([Course_Id])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[CourseFamily] CHECK CONSTRAINT [FK_dbo.CourseFamily_dbo.Course_Course_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Faculty_dbo.Company_Company_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Faculty]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Faculty_dbo.Company_Company_Id] FOREIGN KEY([Company_Id])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Faculty] CHECK CONSTRAINT [FK_dbo.Faculty_dbo.Company_Company_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Faculty_Module_dbo.Faculty_Faculty_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Faculty_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Faculty_Module_dbo.Faculty_Faculty_Id] FOREIGN KEY([Faculty_Id])
REFERENCES [dbo].[Faculty] ([Id])
GO
ALTER TABLE [dbo].[Faculty_Module] CHECK CONSTRAINT [FK_dbo.Faculty_Module_dbo.Faculty_Faculty_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Faculty_Module_dbo.Module_Module_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Faculty_Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Faculty_Module_dbo.Module_Module_Id] FOREIGN KEY([Module_Id])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[Faculty_Module] CHECK CONSTRAINT [FK_dbo.Faculty_Module_dbo.Module_Module_Id]
GO
/****** Object:  ForeignKey [FK_dbo.FeePlan_dbo.Company_Type_Company_Type_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[FeePlan]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FeePlan_dbo.Company_Type_Company_Type_Id] FOREIGN KEY([Company_Type_Id])
REFERENCES [dbo].[Company_Type] ([Id])
GO
ALTER TABLE [dbo].[FeePlan] CHECK CONSTRAINT [FK_dbo.FeePlan_dbo.Company_Type_Company_Type_Id]
GO
/****** Object:  ForeignKey [FK_dbo.FeePlan_Detail_dbo.FeePlan_FeePlan_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[FeePlan_Detail]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FeePlan_Detail_dbo.FeePlan_FeePlan_Id] FOREIGN KEY([FeePlan_Id])
REFERENCES [dbo].[FeePlan] ([Id])
GO
ALTER TABLE [dbo].[FeePlan_Detail] CHECK CONSTRAINT [FK_dbo.FeePlan_Detail_dbo.FeePlan_FeePlan_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Module_dbo.CourseFamily_CourseFamily_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Module]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Module_dbo.CourseFamily_CourseFamily_Id] FOREIGN KEY([CourseFamily_Id])
REFERENCES [dbo].[CourseFamily] ([Id])
GO
ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_dbo.Module_dbo.CourseFamily_CourseFamily_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Prize_dbo.Class_Module_StudentExam_Exam_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Prize]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Prize_dbo.Class_Module_StudentExam_Exam_Id] FOREIGN KEY([Exam_Id])
REFERENCES [dbo].[Class_Module_StudentExam] ([Id])
GO
ALTER TABLE [dbo].[Prize] CHECK CONSTRAINT [FK_dbo.Prize_dbo.Class_Module_StudentExam_Exam_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Prize_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Prize]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Prize_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Prize] CHECK CONSTRAINT [FK_dbo.Prize_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Resource_dbo.Company_Company_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Resource_dbo.Company_Company_Id] FOREIGN KEY([Company_Id])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_dbo.Resource_dbo.Company_Company_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_dbo.Class_Class_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_dbo.Class_Class_Id] FOREIGN KEY([Class_Id])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_dbo.Student_dbo.Class_Class_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_dbo.CourseFamily_CourseFamily_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_dbo.CourseFamily_CourseFamily_Id] FOREIGN KEY([CourseFamily_Id])
REFERENCES [dbo].[CourseFamily] ([Id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_dbo.Student_dbo.CourseFamily_CourseFamily_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_AcademicStatus_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_AcademicStatus]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_AcademicStatus_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Student_AcademicStatus] CHECK CONSTRAINT [FK_dbo.Student_AcademicStatus_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_ClassHistory_dbo.Class_Class_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_ClassHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_ClassHistory_dbo.Class_Class_Id] FOREIGN KEY([Class_Id])
REFERENCES [dbo].[Class] ([Id])
GO
ALTER TABLE [dbo].[Student_ClassHistory] CHECK CONSTRAINT [FK_dbo.Student_ClassHistory_dbo.Class_Class_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_ClassHistory_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_ClassHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_ClassHistory_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Student_ClassHistory] CHECK CONSTRAINT [FK_dbo.Student_ClassHistory_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_FeePlan_dbo.FeePlan_FeePlan_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_FeePlan_dbo.FeePlan_FeePlan_Id] FOREIGN KEY([FeePlan_Id])
REFERENCES [dbo].[FeePlan] ([Id])
GO
ALTER TABLE [dbo].[Student_FeePlan] CHECK CONSTRAINT [FK_dbo.Student_FeePlan_dbo.FeePlan_FeePlan_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_FeePlan_dbo.Student_Student_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_FeePlan_dbo.Student_Student_Id] FOREIGN KEY([Student_Id])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Student_FeePlan] CHECK CONSTRAINT [FK_dbo.Student_FeePlan_dbo.Student_Student_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_FeePlan_Installment_dbo.FeePlan_Detail_FeePlan_Detail_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan_Installment]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_FeePlan_Installment_dbo.FeePlan_Detail_FeePlan_Detail_Id] FOREIGN KEY([FeePlan_Detail_Id])
REFERENCES [dbo].[FeePlan_Detail] ([Id])
GO
ALTER TABLE [dbo].[Student_FeePlan_Installment] CHECK CONSTRAINT [FK_dbo.Student_FeePlan_Installment_dbo.FeePlan_Detail_FeePlan_Detail_Id]
GO
/****** Object:  ForeignKey [FK_dbo.Student_FeePlan_Installment_dbo.Student_FeePlan_Student_FeePlan_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[Student_FeePlan_Installment]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Student_FeePlan_Installment_dbo.Student_FeePlan_Student_FeePlan_Id] FOREIGN KEY([Student_FeePlan_Id])
REFERENCES [dbo].[Student_FeePlan] ([Id])
GO
ALTER TABLE [dbo].[Student_FeePlan_Installment] CHECK CONSTRAINT [FK_dbo.Student_FeePlan_Installment_dbo.Student_FeePlan_Student_FeePlan_Id]
GO
/****** Object:  ForeignKey [FK_dbo.User_Profile_dbo.Role_Type_Role_Type_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[User_Profile]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_Profile_dbo.Role_Type_Role_Type_Id] FOREIGN KEY([Role_Type_Id])
REFERENCES [dbo].[Role_Type] ([Id])
GO
ALTER TABLE [dbo].[User_Profile] CHECK CONSTRAINT [FK_dbo.User_Profile_dbo.Role_Type_Role_Type_Id]
GO
/****** Object:  ForeignKey [FK_dbo.User_Profile_dbo.User_Type_User_Type_Id]    Script Date: 07/24/2016 19:32:07 ******/
ALTER TABLE [dbo].[User_Profile]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_Profile_dbo.User_Type_User_Type_Id] FOREIGN KEY([User_Type_Id])
REFERENCES [dbo].[User_Type] ([Id])
GO
ALTER TABLE [dbo].[User_Profile] CHECK CONSTRAINT [FK_dbo.User_Profile_dbo.User_Type_User_Type_Id]
GO
