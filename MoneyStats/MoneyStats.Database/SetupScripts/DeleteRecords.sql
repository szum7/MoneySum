delete from dbo.[TransactionTagConn];
delete from dbo.[Transaction];
delete from dbo.[Tag];
delete from dbo.[Setting];
delete from dbo.[Currency];
delete from dbo.[User];

DBCC CHECKIDENT ([TransactionTagConn], RESEED, 0)
DBCC CHECKIDENT ([Transaction], RESEED, 0)
DBCC CHECKIDENT ([Tag], RESEED, 0)
DBCC CHECKIDENT ([Setting], RESEED, 0)
DBCC CHECKIDENT ([User], RESEED, 0)