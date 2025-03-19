create database employee;
use employee;

create table emp (
    empno int primary key,
    ename varchar(50) not null,
    job varchar(50),
    mgr_id int,
    hiredate date,
    sal decimal(10,2),
    comm decimal(10,2),
    deptno int
);

create table dept (
    deptno int primary key,
    dname varchar(50),
    loc varchar(50)
);

insert into emp values
(7369, 'smith', 'clerk', 7902, '1980-12-17', 800, null, 20),
(7499, 'allen', 'salesman', 7698, '1981-02-20', 1600, 300, 30),
(7521, 'ward', 'salesman', 7698, '1981-02-22', 1250, 500, 30),
(7566, 'jones', 'manager', 7839, '1981-04-02', 2975, null, 20),
(7654, 'martin', 'salesman', 7698, '1981-09-28', 1250, 1400, 30),
(7698, 'blake', 'manager', 7839, '1981-05-01', 2850, null, 30),
(7782, 'clark', 'manager', 7839, '1981-06-09', 2450, null, 10),
(7788, 'scott', 'analyst', 7566, '1987-04-19', 3000, null, 20),
(7839, 'king', 'president', null, '1981-11-17', 5000, null, 10),
(7844, 'turner', 'salesman', 7698, '1981-09-08', 1500, 0, 30),
(7876, 'adams', 'clerk', 7788, '1987-05-23', 1100, null, 20),
(7900, 'james', 'clerk', 7698, '1981-12-03', 950, null, 30),
(7902, 'ford', 'analyst', 7566, '1981-12-03', 3000, null, 20),
(7934, 'miller', 'clerk', 7782, '1982-01-23', 1300, null, 10);

insert into dept values
(10, 'accounting', 'new york'),
(20, 'research', 'dallas'),
(30, 'sales', 'chicago'),
(40, 'operations', 'boston');

select * from emp;
select * from dept;

-- 1. Retrieve a list of MANAGERS.
select ename from emp where job = 'manager';

-- 2. Find out the names and salaries of all employees earning more than 1000 per month.
select ename, sal from emp where sal > 1000;

-- 3. Display the names and salaries of all employees except JAMES.
select ename, sal from emp where ename != 'james';

-- 4. Find out the details of employees whose names begin with ‘S’.
select * from emp where ename like 's%';

-- 5. Find out the names of all employees that have ‘A’ anywhere in their name.
select ename from emp where ename like '%a%';

-- 6. Find out the names of all employees that have ‘L’ as their third character in their name.
select ename from emp where ename like '__l%';

-- 7. Compute daily salary of JONES.
select ename, sal / 30 as daily_salary from emp where ename = 'jones';

-- 8. Calculate the total monthly salary of all employees.
select sum(sal) as total_salary from emp;

-- 9. Print the average annual salary.
select avg(sal * 12) as avg_annual_salary from emp;

-- 10. Select the name, job, salary, and department number of all employees except SALESMAN from department number 30.
select ename, job, sal, deptno from emp where deptno = 30 and job != 'salesman';

