CREATE PROCEDURE [dbo].[GetTransactionsByPeriod]
	@CreditCardId UNIQUEIDENTIFIER,
    @Year INT,         -- Año de las transacciones
    @Month INT,        -- Mes de las transacciones
    @Type INT = NULL   -- Tipo de transacción (opcional: 1: Compra | 2: Pago | NULL se mostraran todos los tipos)
AS
BEGIN
    -- Calcular la fecha de inicio del período en base al ClosingDay
    WITH FilteredDates AS
    (
        SELECT 
            c.Id AS CreditCardId,
            c.ClosingDay,
            -- Fecha de corte del mes anterior (si el ClosingDay es después del día actual)
            CASE 
                WHEN c.ClosingDay <= DAY(EOMONTH(DATEFROMPARTS(@Year, @Month, 1), -1))
                THEN DATEADD(MONTH, -1, DATEFROMPARTS(@Year, @Month, c.ClosingDay))
                ELSE DATEFROMPARTS(@Year, @Month, c.ClosingDay)
            END AS LastClosingDate,
            -- Fecha final del mes actual
            EOMONTH(DATEFROMPARTS(@Year, @Month, 1)) AS PeriodEndDate
        FROM CreditCards c
    )
    SELECT 
        t.Id AS Id,
        t.CreditCardId,
        t.Description,
        t.Amount,
        t.Type,
        t.Date,
        c.CardNumber,
        fd.ClosingDay,
        fd.LastClosingDate,
        fd.PeriodEndDate,
		t.CreatedAt,
		t.UpdatedAt,
		t.DeletedAt
    FROM Transactions t
    INNER JOIN FilteredDates fd ON t.CreditCardId = fd.CreditCardId
    INNER JOIN CreditCards c ON t.CreditCardId = c.Id
    WHERE
		t.CreditCardId = @CreditCardId
        AND t.Date >= fd.LastClosingDate -- Desde la última fecha de corte
        AND t.Date <= fd.PeriodEndDate -- Hasta el fin del período dado
        AND (@Type IS NULL OR t.Type = @Type) -- Filtro opcional por tipo
    ORDER BY t.Date DESC; -- Orden por fecha descendente
END;
