create database capstone_mwd
use capstone_mwd
use master
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
	account_img varchar(200),
	account_bankName varchar(100),
	account_bankNum varchar(100),
	company_id int foreign key references tbl_companies(company_id)
)

create table tbl_personalInformations(
	personal_id int primary key identity,
	personal_firstName varchar(50),
	personal_lastName varchar(50),
	account_id int foreign key references tbl_accounts(account_id)
)

create table tbl_requirements(
	requirement_id int primary key identity,
	requirement_name varchar(100),
	requirement_dir varchar(255),
	account_id int foreign key references tbl_accounts(account_id)
)

create table tbl_products(
	product_id int primary key identity,
	product_name  varchar(100),
	product_desc  varchar(500),
	product_price float,
	product_owner  int foreign key references tbl_accounts(account_id),
	product_quantity int,
	prod_img varchar(100)
)

create table tbl_threads(
	thread_id int primary key identity,
	thread_title varchar(100),
)

create table tbl_inquiries(
	inq_id int primary key identity,
	inq_content varchar(100),
	inq_date datetime,
	inq_from int foreign key references tbl_accounts(account_id),
	inq_to int foreign key references tbl_accounts(account_id),
	thread_id int foreign key references tbl_threads(thread_id),
)

create table tbl_transactions(
	trans_id int primary key identity,
	trans_date datetime,
	trans_product int foreign key references tbl_products(product_id),
	trans_buyer int foreign key references tbl_accounts(account_id),
	trans_quantity int,
	trans_discount float
)

create table tbl_payment(
	payment_id int primary key identity,
	payment_date datetime,
	trans_id int foreign key references tbl_transactions(trans_id),
	payment_type varchar(10),
	payment_status varchar(10)
)

create table tbl_ads(
	ad_id int primary key identity,
	ad_name varchar(100),
	ad_StartDate varchar(10),
	ad_EndDate varchar(10),
	ad_page varchar(100),
	ad_image varchar(100)
)

insert into tbl_companies values('users','NA')



select * from tbl_accounts
select * from tbl_companies
select * from tbl_inquiries
select * from tbl_products


insert into tbl_accounts values ('admin@gmail.com','1','admin','1',null)
insert into tbl_accounts values ('vendor@gmail.com','1','vendor','2',null)
insert into tbl_accounts values ('distributor@gmail.com','1','distributor','3',null)
insert into tbl_accounts values ('customer@gmail.com','1','customer','4',null)

insert into tbl_companies values('users','NA')

delete from tbl_companies where account_id > 4

select * from tbl_companies
select * from tbl_accounts
select * from tbl_requirements
select * from tbl_personalInformations

delete from tbl_requirements
delete from tbl_accounts


update tbl_accounts set account_status = 1 where account_id = 18
select * from tbl_accounts
where
account_status !=  '1'

select * from tbl_accounts, tbl_companies
where
tbl_accounts.company_id = tbl_companies.company_id

select * from tbl_ads