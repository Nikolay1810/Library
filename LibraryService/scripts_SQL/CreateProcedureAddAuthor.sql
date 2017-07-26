create proc [dbo].[InsertAuthor] 
@firstName varchar(30),
@lastName varchar(30),
@authorId int out
as
insert into Author(FirstName, LastName) values(@firstName, @lastName);
set @authorId = (SELECT IDENT_CURRENT('Author'));
return @authorId;
 
go