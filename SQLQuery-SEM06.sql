USE NeptunoDB
GO
SELECT name, type_desc 
FROM sys.procedures 
WHERE name LIKE 'usp_%';
