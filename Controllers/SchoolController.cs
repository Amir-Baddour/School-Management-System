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
            using (SchoolEntities DB = new SchoolEntities())
            {
                var student = DB.Student.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student); // Will render EditStudent.cshtml with data
            }
        }
        //post editting
        //This one is used to handle form submission
        /*[HttpPost]
        public ActionResult EditStudent(StudentModel updatedStudent)
        {
            using (SchoolEntities DB = new SchoolEntities())
            {
                var student = DB.Student.FirstOrDefault(s => s.Id == updatedStudent.Id);
                if (student != null)
                {
                    student.Name = updatedStudent.Name;
                    student.Gender = updatedStudent.Gender;
                    student.Address = updatedStudent.Address;
                    student.email = updatedStudent.email;
                    student.password = updatedStudent.password;

                     DB.SaveChanges();
                   
                }
            }

            return RedirectToAction("DisplayStudents");
        }*/
        [HttpPost]
        public ActionResult EditStudent(Student student) // Change parameter to match your entity
        {
            using (SchoolEntities DB = new SchoolEntities())
            {
                try
                {
                    var dbStudent = DB.Student.FirstOrDefault(s => s.Id == student.Id);
                    if (dbStudent != null)
                    {
                        dbStudent.Name = student.Name;
                        dbStudent.Gender = student.Gender;
                        dbStudent.Address = student.Address;
                        dbStudent.email = student.email;
                        dbStudent.password = student.password;
                        DB.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // Get the error messages
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    // Join the messages and log them or display to user
                    string fullErrorMessage = string.Join("; ", errorMessages);

                    // You can return this to the view or log it
                    ModelState.AddModelError("", "Validation errors: " + fullErrorMessage);
                    return View(student);
                }
            }
            return RedirectToAction("DisplayStudents");
        }

        //display students
        public ActionResult DisplayStudents()
        {
            using (SchoolEntities DB = new SchoolEntities())
            {
                var studentList = DB.Student.ToList();
                return View(studentList); // <- This will pass a List<Students> to the view
            }
        }



        //delete
        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            using (SchoolEntities db = new SchoolEntities())
            {
                // var student = db.Student.Where(s => s.Id == id).FirstOrDefault();

                var student = db.Student.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                db.Student.Remove(student);
                db.SaveChanges();
                return Json(new { success = true, message = "Student deleted successfully." });
            }
        }

        //Submitting form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult submittingForm(StudentModel student)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return Json(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = errors
                });
            }

            if (student.password.Length < 8)
            {
                return Json(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = new List<string> { "Password must be at least 8 characters." }
                });
            }

            using (SchoolEntities db = new SchoolEntities())
            {
                try
                {
                    var existingStudent = db.Student.FirstOrDefault(n => n.email == student.email);
                    if (existingStudent != null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Email already exists!",
                            errors = new List<string> { "This email is already registered. Please use a different email." }
                        });
                    }

                    Student s = new Student
                    {
                        Name = student.Name,
                        Gender = student.Gender,
                        Address = student.Address,
                        EnrollmentDate = student.EnrollmentDate,
                        email = student.email,
                        password = student.password
                    };

                    db.Student.Add(s);
                    db.SaveChanges();

                    return Json(new { success = true, message = "Student added successfully." });
                }
                catch (DbUpdateException ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Database error",
                        errors = new List<string> { ex.Message }
                    });
                }

            }


        }


    }


}