-- create the database 
create database financemanagement;
go

use financemanagement;
go

create table users (
    userid int identity(1,1) primary key,
    username nvarchar(60),
    email nvarchar(100),
    password nvarchar(100),
);

create table expensecategories (
    categoryid int identity(1,1) primary key,
    categoryname nvarchar(100),
);

create table expenses (
    expenseid int identity(1,1) primary key,
    userid int ,
    categoryid int ,
    amount decimal(10,2),
    description nvarchar(255) ,
    expensedate datetime default getdate(),
    foreign key (userid) references users(userid),
    foreign key (categoryid) references expensecategories(categoryid)
);

insert into users (username, email, password) 
values 
    ('sanjay', 'sanjay432@gmail.com', '12345'),
    ('dhanush', 'dhanush3343@gmail.com', '34534'),
    ('kalki', 'kalki232@gmail.com', '23123');

insert into expensecategories (categoryname) 
values 
    ('food'),
    ('transport'),
    ('shopping'),
    ('utilities'),
    ('entertainment');

insert into expenses (userid, categoryid, amount, description, expensedate) 
values 
    (1, 1, 500.00, 'lunch at annapoorana', getdate()),
    (2, 2, 150.00, 'cab fare', getdate()),
    (1, 3, 1200.00, 'new clothes', getdate()),
    (3, 4, 3500.00, 'monthly electricity bill', getdate());

select table_name 
from information_schema.tables 
where table_type = 'base table';

-- view all data
select * from users;
select * from expensecategories;
select * from expenses;


