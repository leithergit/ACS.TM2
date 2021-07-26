USE [Amadeus5]
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'Timeout'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'AcsPort'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'AcsHost'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'ProcessPath'

GO

ALTER TABLE [dbo].[ACS_DBEvent] DROP CONSTRAINT [DF__ACS_DBEve__Timeo__13A8A87F]
GO

ALTER TABLE [dbo].[ACS_DBEvent] DROP CONSTRAINT [DF__ACS_DBEven__Port__4C4C28FE]
GO

/****** Object:  Table [dbo].[ACS_DBEvent]    Script Date: 2019/4/14 17:53:09 ******/
DROP TABLE [dbo].[ACS_DBEvent]
GO

/****** Object:  Table [dbo].[ACS_DBEvent]    Script Date: 2019/4/14 17:53:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ACS_DBEvent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessPath] [varchar](1024) NOT NULL,
	[AcsHost] [varchar](32) NOT NULL,
	[AcsPort] [varchar](16) NOT NULL,
	[Timeout] [varchar](16) NOT NULL,
 CONSTRAINT [PK_ACS_DBEvent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ACS_DBEvent] ADD  CONSTRAINT [DF__ACS_DBEven__Port__4C4C28FE]  DEFAULT ('5085') FOR [AcsPort]
GO

ALTER TABLE [dbo].[ACS_DBEvent] ADD  DEFAULT ((500)) FOR [Timeout]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DBEvent.exe所在路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'ProcessPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MagoACS软件运行的主机IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'AcsHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MagoACS软件运行的主机服务端口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'AcsPort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'连接超时值,单位毫秒' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ACS_DBEvent', @level2type=N'COLUMN',@level2name=N'Timeout'
GO

