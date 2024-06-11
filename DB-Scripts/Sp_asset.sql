USE [ODW]
GO
/****** Object:  StoredProcedure [dbo].[GetAssets_Pager]    Script Date: 12/7/2019 5:46:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		UmerAslam
-- Create date: 12/7/19
-- Description:	paging on dimasset and search functionality
-- =============================================
ALTER PROCEDURE [dbo].[GetAssets_Pager]
       @SearchTerm VARCHAR(100) = ''
      ,@PageIndex INT = 1
      ,@PageSize INT = 10
      ,@RecordCount INT OUTPUT
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY a.ASSET_KEY ASC
      )AS RowNumber
      ,a.ASSET_KEY
      ,a.ASSETNUM
      ,a.CHANGEBY
      ,a.[DESCRIPTION]
	  ,a.[HIERARCHYPATH]
	  ,a.[STATUS]
	  ,a.[LATITUDEY]
	  ,a.[LONGITUDEX]
	  ,a.[GISLONG]
	  ,a.[GISLAT]
	  ,l.[LOCATION]
      INTO #Results
      FROM [ODW].[EAMS].[vwDimAsset] a left join EAMS.DimLocation l on a.LOCATION_KEY=l.LOCATION_KEY
      WHERE a.STATUS = 'OPERATING' and ASSETNUM LIKE @SearchTerm + '%' OR @SearchTerm = ''
      SELECT @RecordCount = COUNT(*)
      FROM #Results
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
    
      DROP TABLE #Results
END
