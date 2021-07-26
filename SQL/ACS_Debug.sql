USE [Amadeus5]
GO
DECLARE @RC int
DECLARE @LogID int
DECLARE @InsertTime datetime
DECLARE @ReaderID int
DECLARE @CardholderID int
set @LogID  = 99270
set @InsertTime ='2016-11-23 15:55:01';
set @ReaderID =113
set @CardholderID = 93

EXECUTE @RC = [dbo].[ACS_ProcessLogEvent]   @LogID  ,@InsertTime   ,@ReaderID   ,@CardholderID
GO