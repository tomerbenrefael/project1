using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project1.Models;


namespace project1.Controllers
{
    public class ShiftController : Controller
    {
        // GET: Shift
        public ActionResult Index()
        {
            var t = new Project1DBEntities();
            var model = t.Shifts.ToList();
            var employeesListToShift = t.EmployeeShifts.ToList();
            // get list of employees and shifts together
            var EmployeesToShiftDic = new Dictionary<int, List<Employee1>>();
            foreach(var shift in model)
            {
                var employees = employeesListToShift.Where(g => g.ShiftID == shift.ID).Select(g=>g.Employee1).ToList();
                EmployeesToShiftDic.Add(shift.ID, employees);
            }
            ViewBag.EmployeesByShift = EmployeesToShiftDic;
            return View(model);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Shift shift)
        {
            var t = new Project1DBEntities();
            t.Shifts.Add(shift);
            t.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AddShiftToEmployee(int empID)
        {
            var t = new Project1DBEntities();
            var model = new EmployeeShift();
            model.EmployeeID = empID;
            var shifts = t.Shifts.ToList();
            var employeeShifts = t.EmployeeShifts.Where(g => g.EmployeeID == empID).ToList();
            var shiftsToAdd =shifts.Where(g => !employeeShifts.Any(p => p.ShiftID == g.ID)).ToList();
            ViewData.Add("ShiftsList", shiftsToAdd);
            ViewData.Add("EmpID", empID);
            return View(model);

        }
        [HttpPost]
        public ActionResult AddShiftToEmployee(EmployeeShift empShift)
        {
            var t = new Project1DBEntities();
            t.EmployeeShifts.Add(empShift);
            t.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}