﻿namespace EmployeePortal.Models.Entities
{
    public class UpdateEmployeeDto
    {
        public required string? Name { get; set; }

        public required string? Email { get; set; }

        public string? Phone { get; set; }

        public int Salary { get; set; }
    }
}
