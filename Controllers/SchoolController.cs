using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Testing1.Models;

namespace Testing1.Controllers
{
    public class SchoolController : Controller
    {
        // GET: School
        public ActionResult Index()
        {
            ViewBag.UserName = "User";
            return View();
        }
        public ActionResult Student()
        {
            return View();
        }

        //This one is used to show the edit form with pre-filled data
        public ActionResult EditStudent(int id)
        {
            using (SchoolDBEntities1 DB = new SchoolDBEntities1())
            {
                var student = DB.Students.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student); // Will render EditStudent.cshtml with data
            }
        }
        //post editting
        //This one is used to handle form submission
        [HttpPost]
        public ActionResult EditStudent(Students updatedStudent)
        {
            using (SchoolDBEntities1 DB = new SchoolDBEntities1())
            {
                var student = DB.Students.FirstOrDefault(s => s.Id == updatedStudent.Id);
                if (student != null)
                {
                    student.Name = updatedStudent.Name;
                    student.Gender = updatedStudent.Gender;
                    student.Address = updatedStudent.Address;
                    student.email = updatedStudent.email;
                    student.password = updatedStudent.password;

                    try
                    {
                        DB.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine($"Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                            }
                        }
                        throw;
                    }
                }
            }

            return RedirectToAction("DisplayStudents");
        }

        //display students
        public ActionResult DisplayStudents()
        {
            using (SchoolDBEntities1 DB = new SchoolDBEntities1())
            {
                var studentList = DB.Students.ToList();
                return View(studentList); // <- This will pass a List<Students> to the view
            }
        }
        //Submitting form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> submittingForm(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                if (student.password.Length < 8)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Validation failed",
                        errors = new List<string> { "Password must be at least 8 characters." }
                    });
                }

                using (SchoolDBEntities1 DB = new SchoolDBEntities1())
                {
                    Students s = new Students
                    {
                        Name = student.Name,
                        Gender = student.Gender,
                        Address = student.Address,
                        EnrollmentDate = DateTime.Now,
                        email = student.email,
                        password = student.password
                    };

                    DB.Students.Add(s);
                    await DB.SaveChangesAsync(); // asynchronous call
                }

                return Json(new { success = true, message = "Student successfully added!" });
            }

            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, message = "Validation failed", errors = errors });
        }

    }


}