create table Users(
	Id int primary key identity,
	Fullname nvarchar(100) not null,
	Email nvarchar(100) not null unique,
	Password nvarchar(500) not null,
	IsAdmin bit default 0 not null,
	IsActivated bit default 0 not null,
	IsDeleted bit default 0 not null
)

create table Books(
	Id int primary key identity,
	Name nvarchar(100) not null,
	Author nvarchar(100) not null,
	Genre nvarchar(100) not null,
	Quantity int not null,
	Price decimal(10,2) not null
)

create table Customers(
	Id int primary key identity,
	SerialNumber nvarchar(100) not null,
	Fullname nvarchar(100) not null,
	PhoneNumber nvarchar(100) not null
)

create table Sales(
	Id int primary key identity,
	UserId int references Users(Id),
	CustomerId int references Customers(Id),
	Total decimal(10,2) not null,
	StartDate date not null,
	EndDate date not null
)

create table BookSales(
	Id int primary key identity,
	BookId int references Books(Id),
	SaleId int references Sales(Id),
	Quantity int not null
)


alter table Sales add Deadline date not null
alter table Sales add EndDate date

create view SaleDetails
as
select s.Id 'Sale ID', u.Fullname 'Seller',c.Fullname 'Customer', c.PhoneNumber 'Customer Phone',s.StartDate 'Sale Date',s.Deadline,s.EndDate 'Completed Date',s.Total from Sales s
join Customers c
on s.CustomerId=c.Id
join Users u
on s.UserId=u.Id

select * from SaleDetails