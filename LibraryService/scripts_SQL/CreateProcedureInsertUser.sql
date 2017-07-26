create proc [dbo].[InsertUsers] 
@firstName varchar(30),
@lastName varchar(30),
@email varchar(30),
@phoneNumber bigint,
@userId int out
as
insert into Users(FirstName, LastName, Email, PhoneNumber) values(@firstName, @lastName, @email, @phoneNumber);
set @userId = (SELECT IDENT_CURRENT('Users'));
return @userId;
 
go