using EmployeePortal.Data;
using EmployeePortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext DbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary,
            };
            DbContext.Employees.Add(employeeEntity);
            DbContext.SaveChanges();
            return Ok(employeeEntity);
        }

        [HttpGet]
        [Route("GetAllEmployee")]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = DbContext.Employees.ToList();

            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("GetEmployeeById/{Id:int}")]

        public IActionResult GetEmployeeById(int Id)
        {
            var employee = DbContext.Employees.Find(Id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        

        [HttpPut]
        [Route("UpdateEmployeeById/{Id:int}")]

        public IActionResult UpdateEmployeeById(int Id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = DbContext.Employees.Find(Id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            DbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("DeleteEmployeeById/{Id:int}")]

        public IActionResult DeleteEmployeeById(int Id)
        {
            var employee = DbContext.Employees.Find(Id);
            {
                if (employee == null)
                {
                    return NotFound();
                }

                DbContext.Employees.Remove(employee);
                DbContext.SaveChanges();
                return Ok( $"{employee.Name} is Deleted");
            }
        }
    }
}
