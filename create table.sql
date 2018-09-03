create database capstone_mwd
use capstone_mwd
--(localdb)\MSSQLLocalDB

create table tbl_companies(
	company_id int primary key identity,
	company_name varchar(100),
	company_address varchar(200)
)

create table tbl_accounts(
	account_id int primary key identity,
	account_email varchar(40),
	account_status int,
	account_password varchar(40),
	account_type int,
	company_id int foreign key references tbl_companies(company_id)
)

create table tbl_personalInformations(
	personal_id int primary key identity,
	personal_firtName varchar(50),
	personal_lastName varchar(50),
	account_id int foreign key references tbl_accounts(account_id)
)

create table tbl_requirements(
	requirement_id int primary key identity,
	requirement_name varchar(100),
	requirement_dir varchar(255),
	account_id int foreign key references tbl_accounts(account_id)
)

select * from tbl_accounts

insert into tbl_accounts values ('admin@gmail.com','1','admin','1',null)
insert into tbl_accounts values ('vendor@gmail.com','1','vendor','2',null)
insert into tbl_accounts values ('distributor@gmail.com','1','distributor','3',null)
insert into tbl_accounts values ('customer@gmail.com','1','customer','4',null)