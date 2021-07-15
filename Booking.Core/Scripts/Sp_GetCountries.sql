CREATE  PROCEDURE [dbo].[Sp_GetCountries]
	@Keyword [VARCHAR](150),
	@PageIndex [INT] = 1,
	@PageSize [INT] = 10
AS
BEGIN
  WITH CountryList AS 
	(SELECT cnt.Id, cnt.CreatedOn, cnt.Description, cnt.Name, cnt.ModifiedBy, cnt.CreatedBy, cnt.ModifiedOn, cnt.Code,
				COUNT(*) OVER () TotalCount
		FROM COUNTRY cnt WHERE (@Keyword IS NULL OR ((cnt.Name like '%' + @Keyword +'%'))) and cnt.IsDeleted = 0), 
  Counts AS
  (SELECT Count(*) TotalCount
			FROM CountryList)

  	SELECT countryList.*,
		ROW_NUMBER() OVER(ORDER BY countryList.Id DESC) as RowNo,
		c.TotalCount
	FROM CountryList countryList, Counts c
	ORDER BY countryList.ModifiedOn DESC, countryList.CreatedOn DESC
	
    OFFSET (@PageIndex * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY
END
