create database Library;
use Library;

create table Books(
Id int identity primary key,
NameBook varchar(60) not null,
Quantity int not null,
YearPublish int not null,
);

create table Author(
Id int identity primary key,
FirstName varchar(30) not null,
LastName varchar(30) not null,
);

create table BooksAuthors(
Id int identity primary key,
BookId int not null,
AuthorId int not null,
foreign key (BookId) references Books(Id),
foreign key (AuthorId) references Author(Id)
);

create table Users(
Id int identity primary key,
FirstName varchar(30) not null,
LastName varchar(30) not null,
Email varchar(30) not null,
PhoneNumber bigint not null
);

create table History(
Id int identity primary key,
BookId int not null,
UserId int not null,
DateOfIssue date not null,
DateReturn date not null
foreign key (BookId) references Books(Id),
foreign key (UserId) references Users(Id)
);

select * from Books;

insert into Books(NameBook, Quantity, YearPublish) Values ('11 minutes', 5, 2011);
insert into Books(NameBook, Quantity, YearPublish) Values ('Alchemist', 10, 2009);
insert into Books(NameBook, Quantity, YearPublish) Values ('Veronica decides to die', 2, 2012);


insert into Author(FirstName, LastName) Values ('Paulo', 'Coelho');
insert into Author(FirstName, LastName) Values ('Bodo', 'Schaefer');

insert into BooksAuthors(BookId, AuthorId) Values(1, 1);
insert into BooksAuthors(BookId, AuthorId) Values(2, 1);
insert into BooksAuthors(BookId, AuthorId) Values(3, 1);
insert into BooksAuthors(BookId, AuthorId) Values(1, 2);


Select NameBook, Quantity, YearPublish, Author.FirstName, Author.LastName From Books inner join BooksAuthors on BookId = Books.Id inner join Author on AuthorId = Author.Id;

select * from Books where Id = 1004;

select * from Books;

Select Author.Id, FirstName, LastName from Author inner join BooksAuthors on Author.Id = BooksAuthors.AuthorId where BookId = 3;