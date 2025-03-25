-- create database
create database jobportal;
use jobportal;

-- create companies table
create table companies (
    companyid int primary key,
    companyname varchar(255) not null,
    location varchar(255) not null
);

-- create jobs table
create table jobs (
    jobid int primary key,
    companyid int,
    jobtitle varchar(255) not null,
    jobdescription text,
    joblocation varchar(255) not null,
    salary decimal(15,2),
    jobtype varchar(50),
    posteddate datetime,
    foreign key (companyid) references companies(companyid)
);


-- create applicants table
create table applicants (
    applicantid int primary key,
    firstname varchar(100) not null,
    lastname varchar(100) not null,
    email varchar(255) unique not null,
    phone varchar(15),
    resume text
);

-- create applications table
create table applications (
    applicationid int primary key,
    jobid int,
    applicantid int,
    applicationdate datetime,
    coverletter text,
    foreign key (jobid) references jobs(jobid),
    foreign key (applicantid) references applicants(applicantid)
);


insert into companies (companyid, companyname, location) values
(1, 'hexaware technologies', 'mumbai'),
(2, 'tcs', 'chennai'),
(3, 'infosys', 'bangalore');


insert into jobs (jobid, companyid, jobtitle, jobdescription, joblocation, salary, jobtype, posteddate) values
(1, 1, 'software engineer', 'develop and maintain applications.', 'mumbai', 600000.00, 'full-time', '2025-03-01 10:00:00'),
(2, 2, 'data analyst', 'analyze data and provide reports.', 'chennai', 500000.00, 'full-time', '2025-03-02 11:00:00'),
(3, 3, 'project manager', 'manage projects and teams.', 'bangalore', 1200000.00, 'full-time', '2025-03-03 09:00:00');


insert into applicants (applicantid, firstname, lastname, email, phone, resume) values
(1, 'dharanish', 'kalki', 'dharanish.kalki@email.com', '9876543210', 'experienced software engineer with 5 years of experience'),
(2, 'sanjay', 'rao', 'sanjay.rao@email.com', '8765432109', 'data analyst with expertise in sql and python'),
(3, 'kalki', 'sanjay', 'kalki.sanjay@email.com', '7654321098', 'certified project manager with 8 years of experience');


insert into applications (applicationid, jobid, applicantid, applicationdate, coverletter) values
(1, 1, 1, '2025-03-05 14:00:00', 'i am interested in the software engineer position.'),
(2, 2, 2, '2025-03-06 15:00:00', 'excited to apply for the data analyst role.'),
(3, 3, 3, '2025-03-07 16:00:00', 'looking forward to contributing as a project manager.');

alter table applicants add state varchar(100);
update applicants set state = 'maharashtra' where applicantid = 1;
update applicants set state = 'tamil nadu' where applicantid = 2;
update applicants set state = 'karnataka' where applicantid = 3;

select * from companies
select * from jobs
select * from applications
SELECT * FROM applicants

--- 5. Count the number of applications for each job (include jobs with no applications)
select j.jobtitle, count(a.applicationid) as application_count
from jobs j
left join applications a on j.jobid = a.jobid
group by j.jobtitle;

--- 6. Retrieve job listings within a specified salary range
SELECT j.jobtitle, c.companyname, j.joblocation, j.salary
FROM jobs j
JOIN companies c ON j.companyid = c.companyid
WHERE j.salary BETWEEN (SELECT MIN(salary) FROM jobs) AND (SELECT MAX(salary) FROM jobs);

---7. Job application history for a specific applicant
SELECT j.jobtitle, c.companyname, a.applicationdate
FROM applications a
JOIN jobs j ON a.jobid = j.jobid
JOIN companies c ON j.companyid = c.companyid
WHERE a.applicantid = 1;

--- 8. Calculate and display the average salary for job listings (excluding zero salaries)
SELECT AVG(salary) AS average_salary
FROM jobs
WHERE salary > 0;


---9. Identify the company with the most job listings (handling ties)
select companyname, job_count
from (
    select c.companyname, count(j.jobid) as job_count,
           rank() over (order by count(j.jobid) desc) as rnk
    from companies c
    join jobs j on c.companyid = j.companyid
    group by c.companyname
) as rankedjobs
where rnk = 1;

---11. Retrieve distinct job titles with salaries between 600000 and 800000
select distinct jobtitle
from jobs
where salary between 600000 and 800000;

---12. Find jobs without any applications
select j.jobtitle
from jobs j
left join applications a on j.jobid = a.jobid
where a.applicationid is null;

--13. Retrieve job applicants, companies, and applied positions
select a.firstname, a.lastname, c.companyname, j.jobtitle
from applications app
join applicants a on app.applicantid = a.applicantid
join jobs j on app.jobid = j.jobid
join companies c on j.companyid = c.companyid;

---14. Companies with job count even without applications
select c.companyname, count(j.jobid) as job_count
from companies c
left join jobs j on c.companyid = j.companyid
group by c.companyname;

---15. Applicants with companies and positions including non-applicants
select a.firstname, a.lastname, c.companyname, j.jobtitle
from applicants a
left join applications app on a.applicantid = app.applicantid
left join jobs j on app.jobid = j.jobid
left join companies c on j.companyid = c.companyid;

----16. Companies with jobs having salary higher than average
select distinct c.companyname
from companies c
join jobs j on c.companyid = j.companyid
where j.salary > (select avg(salary) from jobs);

----17. Applicants with concatenated city and state (Assuming city and state fields exist)
select firstname, lastname, state as city_state
from applicants;

---18. Jobs with titles containing 'Developer' or 'Engineer'
select jobtitle
from jobs
where jobtitle like '%developer%' or jobtitle like '%engineer%';

-- 19. Applicants and jobs including non-applied jobs and applicants without applications
select a.firstname, a.lastname, j.jobtitle
from applicants a
left join applications app on a.applicantid = app.applicantid
left join jobs j on app.jobid = j.jobid;

-- 20. All combinations of applicants and companies where company is in a specific city and applicant has more than 2 years of experience
select a.firstname, a.lastname, c.companyname
from applicants a
cross join companies c
where c.location = 'chennai' and a.resume like '%2 years%';