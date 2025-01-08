CREATE PROCEDURE [dbo].[AddUserAndCreditCard]
    @FullName NVARCHAR(255),
    @Address NVARCHAR(255),
    @City NVARCHAR(255),
    @State NVARCHAR(255),
    @Country NVARCHAR(255),
    @Email NVARCHAR(255),
    @CardNumber NVARCHAR(50),
    @HolderName NVARCHAR(255),
    @CreditLimit DECIMAL(18, 2),
    @Balance DECIMAL(18, 2),
    @AvailableBalance DECIMAL(18, 2),
    @ClosingDay INT,
    @CreditCardType INT,
	@ConfigurableInterestRate DECIMAL(5,2),
	@ConfigurableMinimumBalanceRate DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insert new user
        DECLARE @NewUserId UNIQUEIDENTIFIER = NEWID();
        DECLARE @NewCardId UNIQUEIDENTIFIER = NEWID();

        INSERT INTO Users (Id, FullName, Address, City, State, Country, Email, CreatedAt)
        VALUES (@NewUserId, @FullName, @Address, @City, @State, @Country, @Email, GETUTCDATE());

        -- Insert new credit card associated with the user
        INSERT INTO CreditCards (Id, CardNumber, HolderName, CreditLimit, Balance, AvailableBalance, ClosingDay, UserId, Type, CreatedAt, ConfigurableInterestRate, ConfigurableMinimumBalanceRate)
        VALUES (@NewCardId, @CardNumber, @HolderName, @CreditLimit, @Balance, @CreditLimit, @ClosingDay, @NewUserId, @CreditCardType, GETUTCDATE(), @ConfigurableInterestRate, @ConfigurableMinimumBalanceRate);

        COMMIT TRANSACTION;

        -- Return the new credit card details
        SELECT 
            [Id],
            [CardNumber],
            [HolderName],
            [CreditLimit],
            [Balance],
            [AvailableBalance],
            [ClosingDay],
			[ConfigurableInterestRate],
			[ConfigurableMinimumBalanceRate],
            [UserId],
            [Type],
            [CreatedAt],
            [UpdatedAt],
            [DeletedAt]
        FROM CreditCards
        WHERE Id = @NewCardId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;
        THROW;
    END CATCH;
END;

