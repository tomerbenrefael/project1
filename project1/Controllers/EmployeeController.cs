using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project1.Models;

namespace project1.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult HomePage(string search)
        {

            var t = new project1.Models.Project1DBEntities();
            List<Models.Employee1> employees = t.Employee1.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                employees = t.Employee1.Include("Department").
                   Where(g => g.FirstName.ToLower().Contains(search.ToLower()) ||
                   g.LastName.ToLower().Contains(search.ToLower())
                   || g.Department.Name.ToLower().Contains(search.ToLower())).ToList();

            }
            return View(employees);
        }


        [Route("{empID}")]
        public ActionResult Edit(int empID)
       {
            var t = new project1.Models.Project1DBEntities();
            var employee = t.Employee1.Include("Department").FirstOrDefault(emp => emp.ID == empID);
            ViewData.Add("DepartmentsList", t.Departments.ToList());
            return View(employee);

        }
        [HttpPost]
        public ActionResult Edit(Employee1 emp1)
        {
            var t = new project1.Models.Project1DBEntities();
            var employee = t.Employee1.Include("Department").FirstOrDefault(emp => emp.ID == emp1.ID);
            employee.FirstName = emp1.FirstName;
            employee.LastName = emp1.LastName;
            employee.StartWorkYear = emp1.StartWorkYear;
            employee.DepartmentID = emp1.DepartmentID;
            t.SaveChanges();
            return RedirectToAction("HomePage");

        }
        [HttpGet]
        public ActionResult Delete(int empID)
        {
            var t = new project1.Models.Project1DBEntities();
            var temp_emp = t.Employee1.First(g => g.ID == empID);
            return View(temp_emp);
        }
        [HttpPost]
        public ActionResult Delete(int empID,int usr)
        {
            var t = new project1.Models.Project1DBEntities();
            var temp_emp = t.Employee1.First(g => g.ID == empID);
            var shifts = t.EmployeeShifts.Where(g => g.EmployeeID == empID).ToList();
            t.Employee1.Remove(temp_emp);
            t.EmployeeShifts.RemoveRange(shifts);
            t.SaveChanges();
            return View(temp_emp);
        }
    }
}