CREATE PROCEDURE [dbo].[GetCreditCardStatement]
    @CreditCardId UNIQUEIDENTIFIER,
	@Year INT,  -- Año del periodo de estado de cuenta
    @Month INT -- Mes del periodo de estado de cuenta
AS
BEGIN

	DECLARE @LastYear INT, 
			@LastMonth INT

	SET @LastYear = CASE WHEN @Month = 1 THEN @Year - 1 ELSE @Year END;
	SET @LastMonth = CASE WHEN @Month = 1 THEN 12 ELSE @Month - 1 END;
    
	-- Variable tipo tabla para almacenar las transacciones del periodo seleccionado
    DECLARE @TransactionsSelectedPeriod TABLE (
        Id UNIQUEIDENTIFIER,
        CreditCardId UNIQUEIDENTIFIER,
        Description NVARCHAR(MAX),
        Amount DECIMAL(18, 2),
        Type INT,
        Date DATETIME,
        CardNumber NVARCHAR(50),
        ClosingDay INT,
        LastClosingDate DATETIME,
        PeriodEndDate DATETIME,
        CreatedAt DATETIME,
        UpdatedAt DATETIME,
        DeletedAt DATETIME
    );

	DECLARE @TransactionsLastPeriod TABLE (
        Id UNIQUEIDENTIFIER,
        CreditCardId UNIQUEIDENTIFIER,
        Description NVARCHAR(MAX),
        Amount DECIMAL(18, 2),
        Type INT,
        Date DATETIME,
        CardNumber NVARCHAR(50),
        ClosingDay INT,
        LastClosingDate DATETIME,
        PeriodEndDate DATETIME,
        CreatedAt DATETIME,
        UpdatedAt DATETIME,
        DeletedAt DATETIME
    );

    INSERT INTO @TransactionsSelectedPeriod
    EXEC [dbo].[GetTransactionsByPeriod] @CreditCardId, @Year, @Month, NULL;

    INSERT INTO @TransactionsLastPeriod
    EXEC [dbo].[GetTransactionsByPeriod] @CreditCardId, @LastYear, @LastMonth, NULL;

	;WITH CTE_TOTAL_PERIODS AS
	(
		SELECT 
			(
				SELECT 
					ISNULL(SUM(selected.Amount),0) 
				FROM @TransactionsSelectedPeriod selected 
				WHERE selected.Type = 1
			) AS 'TotalPurchaseSelectedPeriod',
			(
				SELECT 
					ISNULL(SUM(selected.Amount),0) 
				FROM @TransactionsSelectedPeriod selected 
				WHERE selected.Type = 2
			) AS 'TotalPaymentsSelectedPeriod',
			(
				SELECT 
					ISNULL(SUM(last.Amount),0) 
				FROM @TransactionsLastPeriod last
				WHERE last.Type = 1
			) AS 'TotalPurchaseLastPeriod',
			(
				SELECT 
					ISNULL(SUM(last.Amount),0) 
				FROM @TransactionsLastPeriod last 
				WHERE last.Type = 2
			) AS 'TotalPaymentsLastPeriod'
		) 
		
		SELECT 
			CAR.HolderName,
			CAR.CardNumber,
			CAR.Balance,
			CAR.CreditLimit,
			CAR.AvailableBalance,
			CTE.TotalPurchaseSelectedPeriod,
			CTE.TotalPaymentsSelectedPeriod,
			CTE.TotalPurchaseLastPeriod,
			CTE.TotalPaymentsLastPeriod,
			(CAR.Balance * (CAR.ConfigurableInterestRate)/100) AS 'BonusInterest',
			(CAR.Balance * (CAR.ConfigurableMinimumBalanceRate)/100) AS 'MinimumQuota',
			(CTE.TotalPurchaseSelectedPeriod - CTE.TotalPaymentsSelectedPeriod) as 'TotalPeriodBalance',
			(CAR.Balance + (Balance * (CAR.ConfigurableInterestRate)/100)) AS 'TotalAmountWithInterest'
		FROM CreditCards CAR
		INNER JOIN CTE_TOTAL_PERIODS CTE ON CTE.TotalPurchaseSelectedPeriod IS NOT NULL
		WHERE Id = @CreditCardId
END;