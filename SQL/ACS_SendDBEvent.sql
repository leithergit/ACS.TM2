USE [Amadeus5]
GO

/****** Object:  StoredProcedure [dbo].[ACS_SendDBEvent]    Script Date: 2019/4/14 21:23:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ACS_SendDBEvent]
@strInputString AS varchar(1024) 
AS
BEGIN
	declare @strCommandString varchar(1024)
	DECLARE @strProcessPath varchar(1024),@strACSHost varchar(32),@strTimeout varchar(16)
	DECLARE @strACSPort varchar(16)
	-- DBEvent 192.168.1.121 5085 Input:10240
	SELECT 	@strProcessPath = ProcessPath,
					@strACSHost = AcsHost,
					@strACSPort = AcsPort,
					@strTimeout = Timeout
					from dbo.ACS_DBEvent 
					where ID=1
	set @strCommandString = 'master.sys.xp_cmdshell ''' 
												+ @strProcessPath 
												+ ' ' 
												+ @strACSHost 
												+ ' ' 
												+ @strAcsPort 
												+ ' ' 
												+ @strTimeout
												+ ' '
												+ @strInputString 
												+ ''''
	--Insert into acs_logText  (logText,InsertTime) values (@strCommandString,getdate())
	EXEC(@strCommandString)
	--Insert into acs_logText  (logText,InsertTime) values (@strCommandString,getdate())
END
GO

