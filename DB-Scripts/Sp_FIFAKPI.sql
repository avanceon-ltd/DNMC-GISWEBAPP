USE [ODW]
GO
/****** Object:  StoredProcedure [FIFA].[FIFAKPI]    Script Date: 9/26/2022 2:14:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/

ALTER PROCEDURE [FIFA].[FIFAKPI] @Stadium NVARCHAR(50),@Type NVARCHAR(50)
AS
BEGIN
CREATE TABLE #TEMP (Unassigned INT,Assigned INT,Unattended INT,InProgress INT,WorkComp INT,Resolved INT)
INSERT INTO #TEMP (Unassigned ,Assigned ,Unattended ,InProgress ,WorkComp ,Resolved)
VALUES (
--Unassigned
(CASE @Type WHEN 'Rain' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] 
WHERE StadiumID = @Stadium AND Contact_Reason IN ( 'Rainwater Flooding' ) AND WONUM IS NULL)
WHEN 'NR' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] 
WHERE StadiumID = @Stadium AND Contact_Reason NOT IN ( 'Rainwater Flooding' ) AND WONUM IS NULL) 
ELSE (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] 
WHERE StadiumID = @Stadium AND WONUM IS NULL) END ),
--Assigned
(CASE @Type WHEN 'Rain' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN (  'WAPPR', 'APPR', 'RETURNED', 'RTNSCHED' ))
WHEN 'NR' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason NOT IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN (  'WAPPR', 'APPR', 'RETURNED', 'RTNSCHED' ))
ELSE (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.WONUM IS NOT NULL  AND FW.STATUS IN (  'WAPPR', 'APPR', 'RETURNED', 'RTNSCHED' ))
END ),
--Unattended
(CASE @Type WHEN 'Rain' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ('DISPATCH', 'RTNCRWLD' ))
WHEN 'NR' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason NOT IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ('DISPATCH', 'RTNCRWLD' ))
ELSE (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ('DISPATCH', 'RTNCRWLD' )) END ),
--InProgress
(CASE @Type WHEN 'Rain' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS = 'INPRG')
WHEN 'NR' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason NOT IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS = 'INPRG')
ELSE (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.WONUM IS NOT NULL  AND FW.STATUS = 'INPRG') END ),
--WorkComp
(CASE @Type WHEN 'Rain' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ('WRKCOMP', 'SRVPARRST', 'THDPTY', 'HZMITG', 'QC', 'AUDIT' ))
WHEN 'NR' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason NOT IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ('WRKCOMP', 'SRVPARRST', 'THDPTY', 'HZMITG', 'QC', 'AUDIT' ))
ELSE (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ('WRKCOMP', 'SRVPARRST', 'THDPTY', 'HZMITG', 'QC', 'AUDIT' )) END ),
--Resolved
(CASE @Type WHEN 'Rain' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ( 'CLOSE', 'COMP' ))
WHEN 'NR' THEN (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.Contact_Reason NOT IN ( 'Rainwater Flooding' ) AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ( 'CLOSE', 'COMP' ))
ELSE (SELECT COUNT(*) FROM [ODW].[FIFA].[DrainageComplaints] DC JOIN [ODW].[FIFA].[FactWorkOrder] FW ON DC.WONUM = FW.WONUM
WHERE DC.StadiumID = @Stadium AND DC.WONUM IS NOT NULL  AND FW.STATUS IN ( 'CLOSE', 'COMP' )) END ))

SELECT * FROM #TEMP
END
