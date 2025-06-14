using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Data;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var expense = _context.Expenses.Find(id);
            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense model)
        {
            if (ModelState.IsValid)
            {
                _context.Expenses.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Expenses");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }
            return RedirectToAction("Expenses");
        }



        public IActionResult Expenses()
        {
            var expenses = _context.Expenses.ToList();
            decimal totalAmount = expenses.Sum(e => e.value);

            ViewBag.TotalAmount = totalAmount;
            return View(expenses);
        }


        public IActionResult CreateExpenses()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateExpenseForm(Expense model)
        {
            if (ModelState.IsValid)
            {
                _context.Expenses.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Expenses");
            }
            return View("CreateExpenses", model);
        }
    }
}
