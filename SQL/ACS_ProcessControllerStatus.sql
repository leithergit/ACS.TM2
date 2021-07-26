USE [Amadeus5]
GO

/****** Object:  StoredProcedure [dbo].[ACS_ProcessControllerStatus]    Script Date: 2019/4/14 21:23:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ACS_ProcessControllerStatus]
	@ControllerID int,
	@NewStatus int
AS
BEGIN
	DECLARE @RosID int
	DECLARE @strInput VARCHAR (128)
	DECLARE @Result int 
	
				
	DECLARE @RosName VARCHAR (128) 
	DECLARE readerCursor CURSOR
	FOR													--创建游标tempCursor，并定义游标所指向的集合   
		( SELECT    dbo.Reader.Name
			FROM      dbo.Reader			
			WHERE     dbo.Reader.Controller = @ControllerID
		)ORDER BY dbo.Reader.Name
					
	OPEN readerCursor;								--打开游标  
	FETCH NEXT FROM readerCursor INTO @RosName;			--游标读取下一个数据  
	WHILE @@fetch_status = 0                        --游标读取下一个数据的状态，0表示读取成功  
		BEGIN  
			--PRINT @RosName 							
			--定义事件
			--0为刷卡事件，1为用户事件(Insert,Update,Delete)，2为控制器事件(Online,Offline)
			--格式:2,RosName,Status
			--Sample:2,ROS2,0		释义：2号台，离线
			SET @strInput = '"' +  '2,' + @RosName +','
							+ CONVERT ( VARCHAR ( 32 ),@NewStatus )
							+ '"'
			select @strInput as InputString
			exec dbo.ACS_SendDBEvent @strInput
			FETCH NEXT FROM readerCursor INTO @RosName;    --继续用游标读取下一个数据  
		END  
	CLOSE readerCursor;								--关闭游标
	DEALLOCATE readerCursor;	

END
GO

