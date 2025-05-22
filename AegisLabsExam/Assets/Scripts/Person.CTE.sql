-- Check if the stored procedure already exists
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'Proc_GetPersons') AND type = N'P')
BEGIN
    EXEC('
CREATE PROCEDURE Proc_GetPersons
AS
BEGIN
    WITH temp AS (
        SELECT id, name, age
        FROM persons
        WHERE deleted_at IS NULL
    )
    SELECT * FROM temp;
END');
END;

-- Check if the stored procedure already exists
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'Proc_GetPersonsFiltered') AND type = N'P')
BEGIN
    EXEC('
CREATE PROCEDURE Proc_GetPersonsFiltered
    @Age INT
AS
BEGIN
    WITH temp AS (
        SELECT id, name, age
        FROM persons
        WHERE deleted_at IS NULL AND age >= @Age
    )
    SELECT * FROM temp;
END');
END;

-- Check if the inline function already exists
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'Func_GetPersonsFiltered') AND type = N'IF')
BEGIN
    EXEC('
CREATE FUNCTION Func_GetPersonsFiltered(@Age INT)
    RETURNS TABLE
AS
RETURN (
    WITH temp AS (
        SELECT id, name, age
        FROM persons
        WHERE deleted_at IS NULL AND age >= @Age
    )
    SELECT * FROM temp
)');
END;