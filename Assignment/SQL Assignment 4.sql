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

-- 1. Unique departments
select distinct deptno from emp;

-- 2. Employees earning more than 1500 in dept 10 or 30
select ename Employee, sal 'Monthly Salary' from emp 
where sal > 1500 and deptno in (10, 30);

-- 3. Managers or Analysts with specific salary conditions
select ename, job, sal from emp 
where (job = 'MANAGER' or job = 'ANALYST') and sal not in (1000, 3000, 5000);

-- 4. Employees with commission greater than 10% of salary
select ename, sal, comm from emp 
where comm > sal * 0.10;

-- 5. Employees with two Ls in name, in dept 30 or with manager 7782
select ename from emp 
where (ename like '%L%L%' and deptno = 30) or mgr_id = 7782;

-- 6. Employees with experience between 30 and 40 years and total count
select ename, floor(datediff(curdate(), hiredate) / 365) Experience from emp 
where floor(datediff(curdate(), hiredate) / 365) between 30 and 40;

select count(*) TotalEmployees from emp 
where floor(datediff(curdate(), hiredate) / 365) between 30 and 40;

-- 7. Departments in ascending order and employees in descending order
select d.dname, e.ename from emp e 
join dept d on e.deptno = d.deptno 
order by d.dname asc, e.ename desc;

-- 8. Experience of MILLER
select ename, floor(datediff(curdate(), hiredate) / 365) Experience from emp 
where ename = 'MILLER';

-- 9. Employees with name having 5 or more characters
select * from emp 
where length(ename) >= 5;

-- 10. Copy empno and ename of dept 10 employees to emp10
create table emp10 as 
select empno, ename from emp 
where deptno = 10;
