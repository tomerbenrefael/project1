using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project1.Models;

namespace project1.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
    
        public ActionResult Index()
        {
            
            var t = new project1.Models.Project1DBEntities();
            List<Models.Department> departments = t.Departments.ToList();

            return View(departments);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Department dep)
        {
            var t = new project1.Models.Project1DBEntities();
            t.Departments.Add(dep);
            t.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int depID)
        {
            var t = new project1.Models.Project1DBEntities();
            var dep = t.Departments.FirstOrDefault(g => g.ID == depID);
            if(dep!=null)
            {
                return View(dep);
            }
            return View();
        }
       [HttpPost]
       public ActionResult Edit(Department dep)
        {
            var t = new project1.Models.Project1DBEntities();
            var temp_dep = t.Departments.First(g => g.ID == dep.ID);
            temp_dep.Manager = dep.Manager;
            temp_dep.Name = dep.Name;
            t.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int depID)
        {
            var t = new project1.Models.Project1DBEntities();
            var temp_dep = t.Departments.First(g => g.ID == depID);
            return View(temp_dep);
        }
        [HttpPost]
        public ActionResult Delete(int depID, int managerID)
        {
            var t = new project1.Models.Project1DBEntities();
            var temp_dep = t.Departments.First(g => g.ID == depID && g.Manager==managerID);
            t.Departments.Remove(temp_dep);
            t.SaveChanges();
            return View(temp_dep);
        }
    }
}