-- Create Database
CREATE DATABASE FinanceManagement;
GO

-- Use the Database
USE FinanceManagement;
GO

-- Users Table
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE
);

-- ExpenseCategories Table
CREATE TABLE ExpenseCategories (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name NVARCHAR(50) NOT NULL UNIQUE
);

-- Expenses Table
CREATE TABLE Expenses (
    expense_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES Users(user_id) ON DELETE CASCADE,
    category_id INT FOREIGN KEY REFERENCES ExpenseCategories(category_id) ON DELETE CASCADE,
    amount DECIMAL(10,2) NOT NULL,
    expense_date DATE NOT NULL,
    description NVARCHAR(255)
);

-- Insert Data into Users
INSERT INTO Users (username, password, email)
VALUES 
('kalki', 'kalki@123', 'kalki@gmail.com'),
('manoj', 'manoj@456', 'manoj@gmail.com'),
('bharathi', 'bharathi@789', 'bharathi@gmail.com'),
('dharanish', 'dharanish@111', 'dharanish@gmail.com'),
('balaji', 'balaji@222', 'balaji@gmail.com'),
('sanjay', 'sanjay@333', 'sanjay@gmail.com'),
('haini', 'haini@444', 'haini@gmail.com'),
('aarthi', 'aarthi@555', 'aarthi@gmail.com'),
('dhanushkumar', 'dhanush@666', 'dhanushkumar@gmail.com'),
('abirami', 'abirami@777', 'abirami@gmail.com');

-- Insert Data into ExpenseCategories
INSERT INTO ExpenseCategories (category_name) VALUES 
('Food'),
('Travel'),
('Shopping'),
('Rent'),
('Utilities'),
('Education'),
('Healthcare'),
('Entertainment'),
('Groceries'),
('Miscellaneous');

-- Insert Data into Expenses
INSERT INTO Expenses (user_id, category_id, amount, expense_date, description) VALUES 
(1, 1, 300.00, '2025-03-01', 'Dinner at a restaurant'),
(2, 2, 1500.00, '2025-03-03', 'Flight ticket to Delhi'),
(3, 3, 4500.00, '2025-03-05', 'Shopping at the mall'),
(4, 4, 12000.00, '2025-03-07', 'Monthly house rent'),
(5, 5, 900.00, '2025-03-09', 'Electricity bill payment'),
(6, 6, 3500.00, '2025-03-11', 'College tuition fee'),
(7, 7, 1200.00, '2025-03-13', 'Doctor consultation fee'),
(8, 8, 800.00, '2025-03-15', 'Movie tickets'),
(9, 9, 1500.00, '2025-03-17', 'Weekly groceries shopping'),
(10, 10, 450.00, '2025-03-19', 'Miscellaneous expenses');


SELECT * FROM Users;
SELECT * FROM ExpenseCategories;
SELECT * FROM Expenses;
