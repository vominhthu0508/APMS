USE [MyFramework]
GO
/****** Object:  Table [dbo].[Role_Type]    Script Date: 06/30/2016 11:53:37 ******/
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
SET IDENTITY_INSERT [dbo].[Role_Type] ON
INSERT [dbo].[Role_Type] ([Id], [Role_Type_Name], [Status], [Created_Date]) VALUES (1, N'Admin', 0, CAST(0x0000A60F0139D7FD AS DateTime))
INSERT [dbo].[Role_Type] ([Id], [Role_Type_Name], [Status], [Created_Date]) VALUES (2, N'Mod', 0, CAST(0x0000A60F0139D7FD AS DateTime))
INSERT [dbo].[Role_Type] ([Id], [Role_Type_Name], [Status], [Created_Date]) VALUES (3, N'User', 0, CAST(0x0000A60F0139D7FD AS DateTime))
SET IDENTITY_INSERT [dbo].[Role_Type] OFF
/****** Object:  Table [dbo].[User_Type]    Script Date: 06/30/2016 11:53:37 ******/
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
SET IDENTITY_INSERT [dbo].[User_Type] ON
INSERT [dbo].[User_Type] ([Id], [User_Type_Name], [Status], [Created_Date]) VALUES (1, N'GiamDoc', 0, CAST(0x0000A60F0139D7C9 AS DateTime))
INSERT [dbo].[User_Type] ([Id], [User_Type_Name], [Status], [Created_Date]) VALUES (2, N'TruongPhong', 0, CAST(0x0000A60F0139D7C9 AS DateTime))
INSERT [dbo].[User_Type] ([Id], [User_Type_Name], [Status], [Created_Date]) VALUES (3, N'MoiGioi', 0, CAST(0x0000A60F0139D7C9 AS DateTime))
INSERT [dbo].[User_Type] ([Id], [User_Type_Name], [Status], [Created_Date]) VALUES (4, N'ChuyenGia', 0, CAST(0x0000A60F0139D7C9 AS DateTime))
INSERT [dbo].[User_Type] ([Id], [User_Type_Name], [Status], [Created_Date]) VALUES (5, N'SanLienKet', 0, CAST(0x0000A60F0139D7C9 AS DateTime))
SET IDENTITY_INSERT [dbo].[User_Type] OFF
/****** Object:  Table [dbo].[User_Profile]    Script Date: 06/30/2016 11:53:37 ******/
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
	[User_Profile_Email_2] [nvarchar](100) NULL,
	[User_Profile_Avatar] [nvarchar](100) NULL,
	[User_Profile_Facebook] [nvarchar](50) NULL,
	[User_Profile_Viber] [nvarchar](100) NULL,
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
/****** Object:  Table [dbo].[Account]    Script Date: 06/30/2016 11:53:37 ******/
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
/****** Object:  ForeignKey [FK_dbo.Account_dbo.User_Profile_User_Profile_Id]    Script Date: 06/30/2016 11:53:37 ******/
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Account_dbo.User_Profile_User_Profile_Id] FOREIGN KEY([User_Profile_Id])
REFERENCES [dbo].[User_Profile] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_dbo.Account_dbo.User_Profile_User_Profile_Id]
GO
/****** Object:  ForeignKey [FK_dbo.User_Profile_dbo.Role_Type_Role_Type_Id]    Script Date: 06/30/2016 11:53:37 ******/
ALTER TABLE [dbo].[User_Profile]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_Profile_dbo.Role_Type_Role_Type_Id] FOREIGN KEY([Role_Type_Id])
REFERENCES [dbo].[Role_Type] ([Id])
GO
ALTER TABLE [dbo].[User_Profile] CHECK CONSTRAINT [FK_dbo.User_Profile_dbo.Role_Type_Role_Type_Id]
GO
/****** Object:  ForeignKey [FK_dbo.User_Profile_dbo.User_Type_User_Type_Id]    Script Date: 06/30/2016 11:53:37 ******/
ALTER TABLE [dbo].[User_Profile]  WITH CHECK ADD  CONSTRAINT [FK_dbo.User_Profile_dbo.User_Type_User_Type_Id] FOREIGN KEY([User_Type_Id])
REFERENCES [dbo].[User_Type] ([Id])
GO
ALTER TABLE [dbo].[User_Profile] CHECK CONSTRAINT [FK_dbo.User_Profile_dbo.User_Type_User_Type_Id]
GO
