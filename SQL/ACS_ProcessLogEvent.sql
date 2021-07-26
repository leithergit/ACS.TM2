USE [Amadeus5]
GO

/****** Object:  StoredProcedure [dbo].[ACS_ProcessLogEvent]    Script Date: 2019/4/14 21:23:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ACS_ProcessLogEvent]
	@LogID int,
	@InsertTime datetime,
	--@From_Name varchar(64),	-- 对应logt表中的From_Name
	--@ReqName varchar(250),--对应logt 表中的Desc3字段
	@ReaderID int,
	@CardholderID int
AS
BEGIN
	
	DECLARE @strInput varchar(1024)
	DECLARE @DeviceName VARCHAR(128) 
	DECLARE @Number INT 
	DECLARE @UserName VARCHAR(128)
	DECLARE @SwipteTime datetime;
	
	DECLARE @Firstname VARCHAR(64)
	DECLARE @LastName VARCHAR(64)
	SELECT @Number=dbo.CRDHLD.Num,
					@FirstName= dbo.CRDHLD.First_Name,
					@LastName = dbo.CRDHLD.Last_Name
	FROM dbo.CRDHLD 
	where CRDHLD.ID = @CardholderID;
	
	if LEN(@Firstname) > 0 and LEN(@LastName) > 0
		Set @UserName=@FirstName +' ' + @LastName
	else if LEN(@Firstname) > 0
		Set @UserName=@FirstName 
	else if LEN(@LastName) > 0
		Set @UserName= @LastName
	
	SELECT @SwipteTime = dbo.LOGt.[Date] from dbo.LOGt WHERE LOGt.ID = @LogID;
	--定义事件
	--0为刷卡事件，1为用户事件(Insert,Update,Delete)，2为控制器事件(Online,Offline)
	--格式：0,RosName,EmployeeID,UserName,SwipeTime
	--Sample：0,ROS1,20190411,Test User,2019-04-11 11:20:11
	-- SELECT 	@DeviceID = dbo.ACS_Reader2ROS.ROSID 	FROM 	dbo.ACS_Reader2ROS 	WHERE 	ACS_Reader2ROS.ReaderID = @ReaderID
		SELECT @DeviceName = dbo.Reader.Name FROM dbo.Reader WHERE 	dbo.Reader.ID = @ReaderID
			SET @strInput = '"' +  '0,' + @DeviceName+','
							+ CONVERT ( VARCHAR ( 32 ),@Number )+','
							+ @UserName +','
							+ CONVERT ( VARCHAR ( 32 ),@SwipteTime,20 )
							+ '"'
			select @strInput as InputString
			exec dbo.ACS_SendDBEvent @strInput
END
GO

