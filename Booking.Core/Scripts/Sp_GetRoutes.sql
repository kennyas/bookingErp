CREATE  PROCEDURE [dbo].[Sp_GetRoutes]
	@Keyword [VARCHAR](150),
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS

BEGIN
  WITH routeList AS 
	(SELECT rt.Id, rt.CreatedOn, rt.Description, rt.Name, rt.ModifiedBy, rt.CreatedBy, rt.ModifiedOn, rt.BaseFee, rt.DispatchFee,
				COUNT(*) OVER () TotalCount
		FROM Route rt  
		 WHERE (@Keyword IS NULL OR ((rt.Name like '%' + @Keyword +'%'))) and rt.IsDeleted = 0), 
  Counts AS
  (SELECT Count(*) TotalCount FROM routeList)

   SELECT routeList.*,
		ROW_NUMBER() OVER(ORDER BY routeList.Id DESC) as RowNo,
		c.TotalCount
		FROM RouteList routeList, Counts c
		
		ORDER BY routeList.ModifiedOn DESC, routeList.CreatedOn DESC
	    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END