USE [Amadeus5]
GO

/****** Object:  Trigger [OnUserUpdate]    Script Date: 2019/4/14 17:34:51 ******/
DROP TRIGGER [dbo].[OnUserUpdate]
GO

/****** Object:  Trigger [dbo].[OnUserUpdate]    Script Date: 2019/4/14 17:34:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[OnUserUpdate]
ON [dbo].[CRDHLD]
WITH EXECUTE AS CALLER
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
DECLARE @Operation varchar(32), 	--更新标志，0为插入，1为删除，2为修改
@First_Name VARCHAR(128),
@Last_Name VARCHAR (128),
@EmployeeID int,
@Pin VARCHAR (128),
@PWD VARCHAR(128),
@Department int,
@type tinyint,
@LastDateBadgeGiven datetime,
@NumBadgeGiven int,
@AG_Name VARCHAR(1000)
--新增
if(exists(select 1 from inserted) and not exists(select 1 from deleted))
BEGIN
Set @Operation = 'Insert'
SELECT @Last_Name = Last_Name,@First_Name= First_Name,@EmployeeID = Num,@Pin = Pin,@Department = Department FROM INSERTED
END
--删除
if(not exists(select 1 from inserted) and exists(select 1 from deleted))
BEGIN
Set @Operation = 'Delete'
SELECT @Last_Name = Last_Name,@First_Name= First_Name,@EmployeeID = Num,@Pin = Pin,@Department = Department FROM DELETED
END

if(exists(select 1 from inserted) and exists(select 1 from deleted))
BEGIN

SELECT @Last_Name = Last_Name,@First_Name= First_Name,@EmployeeID = Num,@Pin = Pin,@Department = Department,@type=type,@LastDateBadgeGiven = LastDateBadgeGiven,@NumBadgeGiven= NumBadgeGiven,@AG_Name=AG_Name FROM INSERTED
if (@type=3 and @LastDateBadgeGiven is NULL and @NumBadgeGiven=0 and @AG_Name='')
	Set @Operation = 'Delete'
ELSE
	Set @Operation = 'Update'
END
IF @EmployeeID > 0
BEGIN
	Set @PWD = substring(sys.fn_sqlvarbasetostr(HashBytes('MD5',@Pin)),3,32);
	exec ACS_ProcessUserUpdate @Operation,@First_Name,@Last_Name,@EmployeeID, @PWD,@Department
END
END
GO


