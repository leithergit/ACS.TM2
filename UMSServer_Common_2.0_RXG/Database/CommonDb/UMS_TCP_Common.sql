USE [master]
GO
/****** Object:  Database [UMSTCP]    Script Date: 2020/3/17 14:09:33 ******/
CREATE DATABASE [UMSTCP]
GO
USE [UMSTCP]
GO
/****** Object:  Table [dbo].[AccessList]    Script Date: 2020/3/17 14:09:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessList](
	[RdId] [int] IDENTITY(1,1) NOT NULL,
	[RosId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[LogoutTime] [datetime] NULL,
	[_LogoutType] [int] NOT NULL,
 CONSTRAINT [PK_AccessList] PRIMARY KEY CLUSTERED 
(
	[RdId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BindList]    Script Date: 2020/3/17 14:09:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BindList](
	[RosId] [int] NOT NULL,
	[RosName] [varchar](50) NOT NULL,
	[CurUserName] [varchar](50) NULL,
	[LoginTime] [datetime] NULL,
	[CurUserNo] [varchar](50) NULL,
 CONSTRAINT [PK_RosId] PRIMARY KEY CLUSTERED 
(
	[RosId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2020/3/17 14:09:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDefinition]    Script Date: 2020/3/17 14:09:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDefinition](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserNo] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[UserSex] [int] NOT NULL,
	[UserMobile] [varchar](50) NULL,
	[UserRole] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UserPwd] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UID] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[BindList] ([RosId], [RosName], [CurUserName], [LoginTime], [CurUserNo]) VALUES (1, N'ROS01', NULL, NULL, NULL)
INSERT [dbo].[BindList] ([RosId], [RosName], [CurUserName], [LoginTime], [CurUserNo]) VALUES (2, N'ROS02', NULL, NULL, NULL)
INSERT [dbo].[BindList] ([RosId], [RosName], [CurUserName], [LoginTime], [CurUserNo]) VALUES (3, N'ROS03', NULL, NULL, NULL)
INSERT [dbo].[BindList] ([RosId], [RosName], [CurUserName], [LoginTime], [CurUserNo]) VALUES (4, N'ROS04', NULL, NULL, NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (1, N'DV_Operator')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (2, N'CMS_Operator')
INSERT [dbo].[Role] ([RoleId], [RoleName]) VALUES (3, N'Manager')
SET IDENTITY_INSERT [dbo].[UserDefinition] ON 

INSERT [dbo].[UserDefinition] ([UserId], [UserNo], [UserName], [UserSex], [UserMobile], [UserRole], [CreateDate], [UserPwd]) VALUES (1, N'001', N'DefaultUser', 1, N'1111', 2, CAST(N'2020-03-17T14:16:10.140' AS DateTime), N'e10adc3949ba59abbe56e057f20f883e')
SET IDENTITY_INSERT [dbo].[UserDefinition] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RName]    Script Date: 2020/3/17 14:09:34 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RName] ON [dbo].[BindList]
(
	[RosName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RoleName]    Script Date: 2020/3/17 14:09:34 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_RoleName] ON [dbo].[Role]
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UserNo]    Script Date: 2020/3/17 14:09:34 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserNo] ON [dbo].[UserDefinition]
(
	[UserNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccessList] ADD  CONSTRAINT [DF_AccessList__LogoutType]  DEFAULT ((0)) FOR [_LogoutType]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_RoleName]  DEFAULT ((0)) FOR [RoleName]
GO
ALTER TABLE [dbo].[UserDefinition] ADD  CONSTRAINT [DF_UserDefinition_UserSex]  DEFAULT ((0)) FOR [UserSex]
GO
USE [master]
GO
ALTER DATABASE [UMSTCP] SET  READ_WRITE 
GO
