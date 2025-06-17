

create database shop_db
go

use shop_db
go


create table customer
(
	customerId int identity primary key,
	customerName varchar(30) not null,
	dateOfBirth date not null,
	insideDhaka bit not null,
	picture nvarchar(50) not null
)
go

create table products
(
	productId int identity primary key,
	productName varchar(30) not null,
	manufactureDate date not null,
	sellDate date not null,
	price money not null,
	customerId int not null references customer(customerId)
)
go

select * from customer
select * from products
go


CREATE TABLE department
(
	departmentId INT PRIMARY KEY IDENTITY,
	departmentName VARCHAR(30) NOT NULL
)
go

insert into department(departmentName) values
('Marketing'),
('Sales'),
('Finance '),
('Production'),
('Customer Service')
go


create table sellers
(
	sellerId int primary key,
	sellerName nvarchar(50) not null,
	sellerContact nvarchar(50) not null,
	sellerEmail nvarchar(30) not null,
	picture varbinary(max) null,
	departmentId int references department(departmentId)
)
go



select * from sellers
select * from department
go


--Inner Join
select se.sellerId,se.sellerName,se.sellerEmail,se.sellerContact,se.picture,de.departmentName from sellers se inner join department de on se.departmentId=de.departmentId
go



create table accountants
(
	id INT IDENTITY PRIMARY KEY,
	[name] VARCHAR(50) NOT NULL,
	city VARCHAR(30) NOT NULL,
	department VARCHAR(30) NOT NULL,
	gender VARCHAR(20) NOT NULL
)
go

select * from accountants
go




--Store Procedure
create proc spAccountant
(
	@actId INT=NULL,
	@name VARCHAR(50)=NULL,
	@city  VARCHAR(30)=NULL,
	@department VARCHAR(30)=NULL,
	@gender VARCHAR(20)=NULL,
	@actionType VARCHAR(25)
)
AS
BEGIN
	IF @actionType='SaveData'
	BEGIN
		IF NOT EXISTS (SELECT * FROM accountants WHERE id=@actId)
			BEGIN
				INSERT INTO accountants(name,city,department,gender)
				VALUES(@name,@city,@department,@gender)
			END
		ELSE
			BEGIN
				UPDATE accountants SET name=@name,city=@city,department=@department,gender=@gender WHERE id=@actId
			END
	END
	IF @actionType='DeleteData'
		BEGIN
			DELETE accountants WHERE id=@actId
		END
	IF @actionType='ShowAllData'
		BEGIN
			SELECT * FROM accountants
		END
	IF @actionType='ShowAllDataById'
		BEGIN
			SELECT * FROM accountants WHERE id=@actId
		END
END
GO

