CREATE  PROCEDURE [dbo].[Sp_GetStates]
	@Keyword [VARCHAR](150),
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS

BEGIN
  WITH StateList AS 
	(SELECT st.Id, st.CreatedOn, st.Description, st.Name, st.ModifiedBy, st.CreatedBy, st.ModifiedOn, cnt.name as Country, st.Code as StateCode,
				cnt.Id as CountryId, cnt.Code as CountryCode,
				COUNT(*) OVER () TotalCount
		FROM State st  Join Country cnt on st.CountryId = cnt.Id
		 WHERE (@Keyword IS NULL OR ((st.Name like '%' + @Keyword +'%') OR 
		 (cnt.Name like '%' + @Keyword + '%'))) and cnt.IsDeleted = 0), 
  Counts AS
  (SELECT Count(*) TotalCount FROM StateList)

   SELECT stateList.*,
		ROW_NUMBER() OVER(ORDER BY StateList.Id DESC) as RowNo,
		c.TotalCount
		FROM StateList stateList, Counts c
		
		ORDER BY stateList.ModifiedOn DESC, stateList.CreatedOn DESC
	    OFFSET (@PageIndex) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END