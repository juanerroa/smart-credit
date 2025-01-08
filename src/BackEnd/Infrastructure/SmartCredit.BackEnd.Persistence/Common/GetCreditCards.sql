CREATE PROCEDURE [dbo].[GetCreditCards]
    @CreditCardId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    
	SELECT 
	   [Id]
      ,[CardNumber]
      ,[HolderName]
      ,[CreditLimit]
      ,[Balance]
      ,[AvailableBalance]
      ,[ClosingDay]
	  ,[ConfigurableInterestRate]
      ,[ConfigurableMinimumBalanceRate]
      ,[UserId]
      ,[Type]
      ,[CreatedAt]
      ,[UpdatedAt]
      ,[DeletedAt]
	FROM CreditCards
	WHERE @CreditCardId IS NULL OR Id = @CreditCardId

END

