USE [Amadeus5]
GO

/****** Object:  StoredProcedure [dbo].[ACS_ProcessUserUpdate]    Script Date: 2019/4/14 21:23:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ACS_ProcessUserUpdate]
		 @Operation VARCHAR(128),
		 @First_Name VARCHAR(128),
		 @Last_Name VARCHAR (128),		
		 @EmployeeID int,
		 @Pin VARCHAR (128),
		 @Department int	
AS
BEGIN
		DECLARE @strInput VARCHAR(256), @UserName VARCHAR(128)
		--定义事件
		--0为刷卡事件，1为用户事件(Insert,Update,Delete)，2为控制器事件(Online,Offline)
		--格式:1,Insert(Update,Delete),EmployeeID,UserName,Pin,Depaert
		--Sample:1,Insert,123456,Test Lee,1234,1
		if LEN(@First_Name) > 0 AND LEN(@Last_Name) > 0
			Set @UserName = @First_Name + ' ' + @Last_Name
		else if LEN(@First_Name) > 0
			Set @UserName = @First_Name
		else if LEN(@Last_Name) > 0
			Set @UserName = @Last_Name
		
		
		SET @strInput = '"' +  '1,' + @Operation +','
							+ CONVERT ( VARCHAR ( 32 ),@EmployeeID )+','
							+ @UserName +','
							+ @Pin +','
							+ CONVERT ( VARCHAR ( 32 ),@Department ) +','
							+ '"'
		select @strInput as InputString
		exec dbo.ACS_SendDBEvent @strInput
END
GO

