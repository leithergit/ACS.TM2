USE [Amadeus5]
GO

/****** Object:  Trigger [ACS_Trigger_OnLogt]    Script Date: 2019/4/14 17:36:34 ******/
DROP TRIGGER [dbo].[ACS_Trigger_OnLogt]
GO

/****** Object:  Trigger [dbo].[ACS_Trigger_OnLogt]    Script Date: 2019/4/14 17:36:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[ACS_Trigger_OnLogt]
ON [dbo].[LOGt]
AFTER INSERT
AS
 
BEGIN 
	declare @InsertID int
	declare @Trn_Type int
	declare @ReaderID		int
	declare @ControllerID	int
	declare @CardHolderID	int
	declare @InsertTime datetime

	select @InsertID = ID ,@Trn_Type = Trn_Type,@InsertTime = [Date], @ReaderID = Reader,@ControllerID=Controller,@CardHolderID=CardHolder from inserted
	if @Trn_Type = 1 or @Trn_Type= 3
	begin
		/*
		@LogID int,
		--@Trn_Type int,
		--@From_Name varchar(64),	-- 对应logt表中的From_Name
		--@ReqName varchar(250),--对应logt 表中的Desc3字段
		@ReaderID int,
		@ControllerID int,
		@CardholderID int
		*/
		--Insert into Acs_Logtext(LogText,InsertTime) values ('Before Call ACS_ProcessLogEvent',Getdate())
		exec dbo.ACS_ProcessLogEvent @InsertID,@InsertTime,@ReaderID,@CardHolderID
		--Insert into Acs_Logtext(LogText,InsertTime) values ('After Call ACS_ProcessLogEvent',Getdate())
	end
end

GO


