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

-- 1. employees with higher salary than employee with id 7566
select ename from emp
where sal > (select sal from emp where empno = 7566);

-- 2. employees with the same designation as employee with id 7876
select ename, deptno, job from emp
where job = (select job from emp where empno = 7876);

-- 3. employees reporting to manager whose name starts with 'b' or 'c'
select e.ename, e.empno, e.sal from emp e
join emp m on e.mgr_id = m.empno
where m.ename like 'b%' or m.ename like 'c%';

-- 4a. products more expensive than average in their category (northwind database)
select c.categoryname, p.productname, p.unitprice from products p
join categories c on p.categoryid = c.categoryid
where p.unitprice > (select avg(p2.unitprice) from products p2 where p2.categoryid = p.categoryid);

-- 4b. number of discontinued and total products per category (northwind database)
select c.categoryname, 
       sum(case when p.discontinued = 1 then 1 else 0 end) as discontinuedproducts,
       count(*) as totalproducts
from products p
join categories c on p.categoryid = c.categoryid
group by c.categoryname;
