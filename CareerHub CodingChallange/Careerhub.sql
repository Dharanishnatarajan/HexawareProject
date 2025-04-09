create database careerhub; 
go

use careerhub;
go


-- companies table
create table companies (
    companyid int primary key identity(1,1),
    companyname nvarchar(60),
    location nvarchar(60) 
);

-- applicants table
create table applicants (
    applicantid int primary key identity(1,1),
    firstname nvarchar(60),
    lastname nvarchar(60),
    email nvarchar(100),
    phone nvarchar(15),
    resume nvarchar(max)
);

-- jobs table
create table jobs (
    jobid int primary key identity(1,1),
    companyid int,
    jobtitle nvarchar(100),
    jobdescription nvarchar(max),
    joblocation nvarchar(100),
    salary decimal(10,2),
    jobtype nvarchar(100),
    posteddate datetime default getdate(),
    foreign key (companyid) references companies(companyid)
);

-- applications table
create table applications (
    applicationid int primary key identity(1,1),
    jobid int,
    applicantid int,
    applicationdate datetime default getdate(),
    coverletter nvarchar(max),
    foreign key (jobid) references jobs(jobid),
    foreign key (applicantid) references applicants(applicantid)
);

SELECT name 
FROM sys.tables;

select * from companies;
select * from jobs;
select * from applicants;
select * from applications;

