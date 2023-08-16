using CRUD_MVC.Data;
using CRUD_MVC.Models;
using CRUD_MVC.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CrudDbContext _DBContext;
        public EmployeeController(CrudDbContext crudDbContext)
        {
            this._DBContext = crudDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employee = await _DBContext.Employees.ToListAsync();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel employeeViewModel)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = employeeViewModel.Name,
                Email = employeeViewModel.Email,
                Salary = employeeViewModel.Salary,
                Department = employeeViewModel.Department,
                DateOfBirth = employeeViewModel.DateOfBirth
            };
            await _DBContext.Employees.AddAsync(employee);
            await _DBContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> View(Guid Id)
        {
            var employee = await _DBContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);

            if (employee != null)
            {
                var viewModel = new UpdateViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(() => View(("View"),viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateViewModel model)
        {
            var employee = await _DBContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;
                await _DBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateViewModel model)
        {
            var employee = await _DBContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                _DBContext.Employees.Remove(employee);
                await _DBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
