
alter proc [dbo].[InsertBookAndAuthors] 
@bookName varchar(60),
@Quantity int,
@YearPublish int,
@bookId int out
as
insert into Books(NameBook, Quantity, YearPublish) values(@bookName,@Quantity, @YearPublish);
set @bookId = (SELECT IDENT_CURRENT('Books'));
return @bookId;
 
go