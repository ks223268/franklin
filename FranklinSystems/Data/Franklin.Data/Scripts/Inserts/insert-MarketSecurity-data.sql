/*
Insert market securities.
*/

DECLARE @Id INT,
		@Code VARCHAR(10)

	SET @Id = 1

WHILE @Id <= 10
BEGIN			
	SET @Code = CONCAT('AA', FORMAT(@Id, '00'))	
	INSERT INTO [dbo].[MarketSecurity]([Code]) VALUES (@Code)
	
	SET @ID = @Id + 1
END
GO

--DELETE FROM dbo.MarketSecurity
SELECT * FROM dbo.MarketSecurity


