USE [Amadeus5]
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Controller', @level2type=N'TRIGGER',@level2name=N'OnControllerUpdate'

GO

/****** Object:  Trigger [OnControllerUpdate]    Script Date: 2019/4/14 17:33:41 ******/
DROP TRIGGER [dbo].[OnControllerUpdate]
GO

/****** Object:  Trigger [dbo].[OnControllerUpdate]    Script Date: 2019/4/14 17:33:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[OnControllerUpdate]
ON [dbo].[Controller]
WITH EXECUTE AS CALLER
AFTER UPDATE
AS
BEGIN
	DECLARE @ControllerID INT
	DECLARE @NewStatus int 	--0为离线，1为在线
	DECLARE @OldStatus int
  select @ControllerID = ID,@NewStatus=PollingOK from Inserted
	SELECT @OldStatus=PollingOK from deleted
	if @NewStatus <> @OldStatus
		exec ACS_ProcessControllerStatus @ControllerID,@NewStatus
END
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'跟踪控制的状态器变化' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Controller', @level2type=N'TRIGGER',@level2name=N'OnControllerUpdate'
GO


