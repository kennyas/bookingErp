CREATE  PROCEDURE [dbo].[Sp_GetDepartments]
	@Keyword [VARCHAR](150),
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS
BEGIN
  WITH DepartmentList AS 
	(SELECT  dept.Id, dept.CreatedOn, dept.Description, dept.Name, dept.ModifiedBy, dept.CreatedBy, dept.ModifiedOn,
				COUNT(*) OVER () TotalCount
		FROM Department dept WHERE (@Keyword IS NULL OR ((dept.Name like '%' + @Keyword +'%'))) and dept.IsDeleted = 0), 
  Counts AS
  (SELECT Count(*) TotalCount
			FROM DepartmentList)

  	SELECT deptList.*,
		ROW_NUMBER() OVER(ORDER BY deptList.Id DESC) as RowNo,
		c.TotalCount
	FROM DepartmentList deptList, Counts c
	ORDER BY deptList.ModifiedOn DESC, deptList.CreatedOn DESC
	
    OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY
END
